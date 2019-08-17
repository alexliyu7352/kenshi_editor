using NLog;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace forgotten_construction_set
{
	internal class SteamManager
	{
		public static string SteamWorkshopAgreementURL;

		private static SteamManager instance;

		private ModData uploadingMod;

		private Action<bool, string> uploadCallback;

		private UGCUpdateHandle_t uploadUpdateHandle = new UGCUpdateHandle_t((long)0);

		private SteamManager.UploadState uploadState;

		private Action<bool> getModInfoCallback;

		private ModData getModInfoMod;

		private AppId_t AppId = new AppId_t(233860);

		private CSteamID userSteamId;

		private HashSet<PublishedFileId_t> publishedContent = new HashSet<PublishedFileId_t>();

		private CallResult<SteamUGCQueryCompleted_t> mOnSteamUGCQueryCompletedUserModsCallResult;

		private CallResult<SteamUGCRequestUGCDetailsResult_t> mOnSteamUGCQueryCompletedModInfoCallResult;

		private CallResult<CreateItemResult_t> mOnCreateItemResultCallResult;

		private CallResult<SubmitItemUpdateResult_t> mOnSubmitItemUpdateResultCallResult;

		private SteamAPIWarningMessageHook_t mWarningMessageHook;

		private Thread updateThread;

		private static Logger logger;

		public bool Enabled
		{
			get;
			private set;
		}

		public static SteamManager Instance
		{
			get
			{
				return SteamManager.instance;
			}
		}

		public bool Uploading
		{
			get;
			private set;
		}

		static SteamManager()
		{
			SteamManager.SteamWorkshopAgreementURL = "http://steamcommunity.com/sharedfiles/workshoplegalagreement";
			SteamManager.instance = new SteamManager();
			SteamManager.logger = LogManager.GetLogger("Steam API");
		}

		private SteamManager()
		{
            // TODO ≤‚ ‘ π”√
			this.Enabled = true;
			this.updateThread = new Thread(new ThreadStart(this.Update))
			{
				IsBackground = true,
				Name = "Steam API Update Thread"
			};
		}

		public void AbortUpload()
		{
			bool uploading = this.Uploading;
			SteamManager.logger.Info("Aborting upload");
			this.uploadState = SteamManager.UploadState.Aborted;
			this.Uploading = false;
			this.uploadCallback = null;
			this.mOnCreateItemResultCallResult.Cancel();
			this.mOnSubmitItemUpdateResultCallResult.Cancel();
			this.uploadingMod = null;
			this.uploadState = SteamManager.UploadState.None;
		}

		private void CreateMod()
		{
			SteamManager.logger.Info("Creating mod '{0}'", this.uploadingMod.mod);
			this.uploadState = SteamManager.UploadState.Creating;
			SteamAPICall_t steamAPICallT = SteamUGC.CreateItem(this.AppId, EWorkshopFileType.k_EWorkshopFileTypeFirst);
			this.mOnCreateItemResultCallResult.Set(steamAPICallT, null);
		}

		public SteamManager.UploadState GetUploadState(out ulong processedBytes, out ulong totalBytes)
		{
			processedBytes = (ulong)0;
			totalBytes = (ulong)0;
			if (this.Uploading)
			{
				SteamUGC.GetItemUpdateProgress(this.uploadUpdateHandle, out processedBytes, out totalBytes);
			}
			return this.uploadState;
		}

		public bool GetUserQuota(out int total, out int available)
		{
			total = 0;
			available = 0;
			return SteamRemoteStorage.GetQuota(out total, out available);
		}

		public void Init()
		{
            if (!this.Enabled)
            {
                SteamManager.logger.Warn("Steam API not enabled");
                return;
            }
            if (!Packsize.Test())
            {
                SteamManager.logger.Error("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.");
                return;
            }
            if (!DllCheck.Test())
			{
				SteamManager.logger.Error("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.");
				return;
			}
			if (!SteamAPI.Init())
			{
				SteamManager.logger.Error("Error initializing Steam API");
				return;
			}
			this.mWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.SteamAPIDebugTextHook);
			SteamClient.SetWarningMessageHook(this.mWarningMessageHook);
			this.mOnSteamUGCQueryCompletedUserModsCallResult = CallResult<SteamUGCQueryCompleted_t>.Create(new CallResult<SteamUGCQueryCompleted_t>.APIDispatchDelegate(this.OnSteamUGCQueryCompletedUserMods));
			this.mOnSteamUGCQueryCompletedModInfoCallResult = CallResult<SteamUGCRequestUGCDetailsResult_t>.Create(new CallResult<SteamUGCRequestUGCDetailsResult_t>.APIDispatchDelegate(this.OnSteamUGCQueryCompletedModInfo));
			this.mOnCreateItemResultCallResult = CallResult<CreateItemResult_t>.Create(new CallResult<CreateItemResult_t>.APIDispatchDelegate(this.OnCreateItemResult));
			this.mOnSubmitItemUpdateResultCallResult = CallResult<SubmitItemUpdateResult_t>.Create(new CallResult<SubmitItemUpdateResult_t>.APIDispatchDelegate(this.OnSubmitItemUpdateResult));
			this.userSteamId = SteamUser.GetSteamID();
			this.Enabled = true;
			SteamManager.logger.Info("Initialized. User Steam ID: {0}", this.userSteamId.m_SteamID);
			SteamAPICall_t steamAPICallT = SteamUGC.SendQueryUGCRequest(SteamUGC.CreateQueryUserUGCRequest(this.userSteamId.GetAccountID(), EUserUGCList.k_EUserUGCList_Published, EUGCMatchingUGCType.k_EUGCMatchingUGCType_All, EUserUGCListSortOrder.k_EUserUGCListSortOrder_TitleAsc, AppId_t.Invalid, this.AppId, 1));
			this.mOnSteamUGCQueryCompletedUserModsCallResult.Set(steamAPICallT, null);
			this.updateThread.Start();
		}

		private void OnCreateItemResult(CreateItemResult_t pCallback, bool bIOFailure)
		{
			if (!this.Uploading || this.uploadingMod == null || this.uploadCallback == null)
			{
				return;
			}
			bool flag = true;
			string empty = string.Empty;
			if (bIOFailure)
			{
				flag = false;
				empty = "Error creating workshop item. Error: I/O failure";
			}
			else if (pCallback.m_eResult == EResult.k_EResultInsufficientPrivilege)
			{
				flag = false;
				empty = "Error creating workshop item. Error: The user creating the item is currently banned in the community.";
			}
			else if (pCallback.m_eResult == EResult.k_EResultTimeout)
			{
				flag = false;
				empty = "Error creating workshop item. Error: Timeout.";
			}
			else if (pCallback.m_eResult == EResult.k_EResultNotLoggedOn)
			{
				flag = false;
				empty = "Error creating workshop item. Error: The user is not currently logged into Steam.";
			}
			else if (pCallback.m_eResult != EResult.k_EResultOK)
			{
				flag = false;
				empty = string.Format("Error creating workshop item. Error value: {0}", pCallback.m_eResult.ToString());
			}
			if (!flag)
			{
				SteamManager.logger.Error<string, string>("Error creating mod '{0}'. Info: {1}", this.uploadingMod.mod, empty);
				this.Uploading = false;
				this.uploadingMod = null;
				this.uploadState = SteamManager.UploadState.None;
				this.uploadCallback(false, empty);
				this.uploadCallback = null;
				return;
			}
			PublishedFileId_t mNPublishedFileId = pCallback.m_nPublishedFileId;
			this.publishedContent.Add(mNPublishedFileId);
			this.uploadingMod.id = mNPublishedFileId.m_PublishedFileId;
			this.uploadingMod.Save();
			SteamManager.logger.Info<string, ulong>("Created mod '{0}' successfully. ID: {1}", this.uploadingMod.mod, this.uploadingMod.id);
			this.UpdateMod();
		}

		private void OnSteamUGCQueryCompletedModInfo(SteamUGCRequestUGCDetailsResult_t pCallback, bool bIOFailure)
		{
			bool flag = false;
			if (!bIOFailure && pCallback.m_details.m_eResult == EResult.k_EResultOK)
			{
				this.getModInfoMod.title = pCallback.m_details.m_rgchTitle;
				this.getModInfoMod.visibility = (int)pCallback.m_details.m_eVisibility;
				this.getModInfoMod.header.Description = pCallback.m_details.m_rgchDescription;
				this.getModInfoMod.tags.Clear();
				this.getModInfoMod.tags.AddRange(pCallback.m_details.m_rgchTags.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
				this.getModInfoMod.lastUpdate = new DateTime?(this.UnixTimeStampToDateTime(pCallback.m_details.m_rtimeUpdated));
				flag = true;
			}
			SteamManager.logger.Debug("Query mod's info complete. Result: {0}", pCallback.m_details.m_eResult.ToString());
			this.getModInfoCallback(flag);
			this.getModInfoCallback = null;
			this.getModInfoMod = null;
		}

		private void OnSteamUGCQueryCompletedUserMods(SteamUGCQueryCompleted_t pCallback, bool bIOFailure)
		{
			SteamUGCDetails_t steamUGCDetailsT;
			if (!bIOFailure && pCallback.m_eResult == EResult.k_EResultOK && pCallback.m_unNumResultsReturned > 0)
			{
				for (uint i = 0; i < pCallback.m_unNumResultsReturned; i++)
				{
					if (SteamUGC.GetQueryUGCResult(pCallback.m_handle, i, out steamUGCDetailsT))
					{
						this.publishedContent.Add(steamUGCDetailsT.m_nPublishedFileId);
					}
				}
			}
			SteamManager.logger.Info("Published user mods IDs: {0}", string.Join<PublishedFileId_t>(", ", this.publishedContent));
			SteamUGC.ReleaseQueryUGCRequest(pCallback.m_handle);
		}

		private void OnSubmitItemUpdateResult(SubmitItemUpdateResult_t pCallback, bool bIOFailure)
		{
			if (!this.Uploading || this.uploadingMod == null || this.uploadCallback == null)
			{
				return;
			}
			bool flag = true;
			string empty = string.Empty;
			if (bIOFailure)
			{
				flag = false;
				empty = "Error updating workshop item. Error: I/O failure";
			}
			else if (pCallback.m_eResult == EResult.k_EResultInsufficientPrivilege)
			{
				flag = false;
				empty = "Error updating workshop item. Error: The user creating the item is currently banned in the community.";
			}
			else if (pCallback.m_eResult == EResult.k_EResultTimeout)
			{
				flag = false;
				empty = "Error updating workshop item. Error: Timeout.";
			}
			else if (pCallback.m_eResult == EResult.k_EResultNotLoggedOn)
			{
				flag = false;
				empty = "Error updating workshop item. Error: The user is not currently logged into Steam.";
			}
			else if (pCallback.m_eResult != EResult.k_EResultOK)
			{
				flag = false;
				empty = string.Format("Error updating workshop item. Error value: {0}", pCallback.m_eResult.ToString());
			}
			if (flag)
			{
				SteamManager.logger.Info<string, ulong>("Updated mod '{0}' successfully. ID: {1}", this.uploadingMod.mod, this.uploadingMod.id);
				this.Uploading = false;
				this.uploadingMod = null;
				this.uploadCallback(true, string.Empty);
				this.uploadCallback = null;
				this.uploadState = SteamManager.UploadState.None;
				return;
			}
			SteamManager.logger.Error<string, ulong, string>("Error updating mod '{0}' (ID: {1}). Info: {1}", this.uploadingMod.mod, this.uploadingMod.id, empty);
			this.Uploading = false;
			this.uploadingMod = null;
			this.uploadState = SteamManager.UploadState.None;
			this.uploadCallback(false, empty);
			this.uploadCallback = null;
		}

		public void ShowSteamWorkshopAgreement()
		{
			if (this.Enabled && SteamUtils.IsOverlayEnabled())
			{
				SteamFriends.ActivateGameOverlayToWebPage(SteamManager.SteamWorkshopAgreementURL);
				return;
			}
			Process.Start(SteamManager.SteamWorkshopAgreementURL);
		}

		public void Shutdown()
		{
            if (this.Enabled)
            {
                try { 

                    SteamManager.logger.Info("Shutting down");
                    this.Enabled = false;
                    this.updateThread.Join();
                    SteamAPI.Shutdown();
                    this.publishedContent.Clear();
                }
                catch(Exception)
                {
                    SteamManager.logger.Info("Shutting down error");
                }
			}
		}

		private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
		{
			SteamManager.logger.Debug<StringBuilder>("[SteamAPIDebugTextHook] {0}", pchDebugText);
		}

		private DateTime UnixTimeStampToDateTime(uint unixTimeStamp)
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			dateTime = dateTime.AddSeconds((double)((float)unixTimeStamp));
			return dateTime.ToLocalTime();
		}

		private void Update()
		{
			while (this.Enabled)
			{
				SteamAPI.RunCallbacks();
				Thread.Sleep(100);
			}
		}

		private void UpdateMod()
		{
			if (!this.Uploading || this.uploadingMod == null || this.uploadCallback == null)
			{
				return;
			}
			SteamManager.logger.Info<string, ulong>("Updating mod '{0}' (ID: {1})", this.uploadingMod.mod, this.uploadingMod.id);
			this.uploadState = SteamManager.UploadState.Updating;
			UGCUpdateHandle_t uGCUpdateHandleT = SteamUGC.StartItemUpdate(this.AppId, new PublishedFileId_t(this.uploadingMod.id));
			SteamUGC.SetItemTitle(uGCUpdateHandleT, this.uploadingMod.title);
			SteamUGC.SetItemDescription(uGCUpdateHandleT, this.uploadingMod.header.Description);
			SteamUGC.SetItemTags(uGCUpdateHandleT, this.uploadingMod.tags);
			SteamUGC.SetItemVisibility(uGCUpdateHandleT, (ERemoteStoragePublishedFileVisibility)this.uploadingMod.visibility);
			string absoluteImagePath = this.uploadingMod.GetAbsoluteImagePath();
			if (!string.IsNullOrWhiteSpace(absoluteImagePath) && File.Exists(absoluteImagePath))
			{
				SteamUGC.SetItemPreview(uGCUpdateHandleT, absoluteImagePath);
			}
			SteamUGC.SetItemMetadata(uGCUpdateHandleT, this.uploadingMod.header.Version.ToString());
			SteamUGC.SetItemContent(uGCUpdateHandleT, this.uploadingMod.GetAbsoluteFolder());
			SteamAPICall_t steamAPICallT = SteamUGC.SubmitItemUpdate(uGCUpdateHandleT, this.uploadingMod.changeNotes);
			this.mOnSubmitItemUpdateResultCallResult.Set(steamAPICallT, null);
			this.uploadUpdateHandle = uGCUpdateHandleT;
		}

		public void UpdateModInfo(ModData data, Action<bool> callback)
		{
			if (data == null)
			{
				return;
			}
			if (data.id == 0)
			{
				callback(false);
				return;
			}
			SteamManager.logger.Info<string, ulong>("Request Steam's mod information for '{0}' (ID: {1})", data.mod, data.id);
			this.getModInfoMod = data;
			this.getModInfoCallback = callback;
			SteamAPICall_t steamAPICallT = SteamUGC.RequestUGCDetails(new PublishedFileId_t(data.id), 5);
			this.mOnSteamUGCQueryCompletedModInfoCallResult.Set(steamAPICallT, null);
		}

		public void Upload(ModData data, Action<bool, string> callback)
		{
			if (this.Uploading)
			{
				return;
			}
			SteamManager.logger.Info<string, ulong>("Uploading mod '{0}' (Id: {1})", data.mod, data.id);
			this.Uploading = true;
			this.uploadingMod = data;
			this.uploadCallback = callback;
			if (this.uploadingMod.id != 0 && !this.publishedContent.Contains(new PublishedFileId_t(this.uploadingMod.id)))
			{
				SteamManager.logger.Warn("Trying to upload a mod (ID: {0}) that doesn't belong to the player or it was deleted.", this.uploadingMod.id);
			}
			if (this.uploadingMod.id == 0)
			{
				this.CreateMod();
				return;
			}
			this.UpdateMod();
		}

		public enum UploadState : byte
		{
			None,
			Creating,
			Updating,
			Aborted
		}
	}
}