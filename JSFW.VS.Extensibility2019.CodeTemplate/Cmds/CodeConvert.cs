using System;
using System.ComponentModel.Design;
using System.Globalization;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace JSFW.VS.Extensibility2019.CodeTemplate.Cmds
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class CodeConvert
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("9e065628-e6e7-473c-8521-5b0667fb2c8c");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package package;

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeConvert"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private CodeConvert(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this.package = package;

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static CodeConvert Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new CodeConvert(package);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            //string message = string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", this.GetType().FullName);
            //string title = "CodeConvert";

            //// Show a message box to prove we were here
            //VsShellUtilities.ShowMessageBox(
            //    this.ServiceProvider,
            //    message,
            //    title,
            //    OLEMSGICON.OLEMSGICON_INFO,
            //    OLEMSGBUTTON.OLEMSGBUTTON_OK,
            //    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
            ShowToolWindow();
        }

        private IVsWindowFrame m_windowFrame = null;
        CodeTemplate.Cmds.Controls.CodeTemplateListUserControl codeTemplateListCtrl = null;

        private void ShowToolWindow()
        {
            const string TOOLWINDOW_GUID = "C53FC666-B883-4788-9C3C-D60799BC7852";

            if (codeTemplateListCtrl == null) codeTemplateListCtrl = new CodeTemplate.Cmds.Controls.CodeTemplateListUserControl();

            codeTemplateListCtrl.ListRefresh();
            codeTemplateListCtrl.ExportToString -= SnippetListCtrl_ExportToString;
            codeTemplateListCtrl.ExportToString += SnippetListCtrl_ExportToString;

            codeTemplateListCtrl.ImportToString -= GetSelection;
            codeTemplateListCtrl.ImportToString += GetSelection;

            if (m_windowFrame == null)
            {
                m_windowFrame = CreateToolWindow("코드변환 템플릿 목록", TOOLWINDOW_GUID, codeTemplateListCtrl);

                // TODO: Initialize m_userControl if required adding a method like:
                //    internal void Initialize(VSPackageToolWindowPackage package)
                // and pass this instance of the package:
                //    m_userControl.Initialize(this);
            }

            // 읽어온 ... 값 snippetListControl에 ... 전달?
            m_windowFrame.Show();
        }

        private void SnippetListCtrl_ExportToString(string txt)
        {
            if (!string.IsNullOrEmpty((txt ?? "").Trim()))
            {
                SetSelection(txt);
            }
        }

        private IVsWindowFrame CreateToolWindow(string caption, string guid, CodeTemplate.Cmds.Controls.CodeTemplateListUserControl snippet)
        {
            const int TOOL_WINDOW_INSTANCE_ID = 0; // Single-instance toolwindow

            IVsUIShell uiShell;
            Guid toolWindowPersistenceGuid;
            Guid guidNull = Guid.Empty;
            int[] position = new int[1];
            int result;
            IVsWindowFrame windowFrame = null;
            uiShell = (IVsUIShell)ServiceProvider.GetService(typeof(SVsUIShell));
            toolWindowPersistenceGuid = new Guid(guid);
            result = uiShell.CreateToolWindow((uint)__VSCREATETOOLWIN.CTW_fInitNew,
                  TOOL_WINDOW_INSTANCE_ID, snippet, ref guidNull, ref toolWindowPersistenceGuid,
                  ref guidNull, null, caption, position, out windowFrame);

            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(result);
            return windowFrame;
        }

        private string GetSelection()
        {
            string setting = "";

            EnvDTE80.DTE2 _applicationObject = ServiceProvider.GetService(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;
            //Check active document
            if (_applicationObject.ActiveDocument != null)
            {
                //Get active document
                EnvDTE.TextDocument objTextDocument = (EnvDTE.TextDocument)_applicationObject.ActiveDocument.Object("");
                EnvDTE.TextSelection objTextSelection = objTextDocument.Selection;

                if (!String.IsNullOrEmpty(objTextSelection.Text))
                {
                    //Get selected text
                    setting = objTextSelection.Text;
                }
            }
            return setting;
        }

        private void SetSelection(string txt)
        {
            EnvDTE80.DTE2 _applicationObject = ServiceProvider.GetService(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;

            //Check active document
            if (_applicationObject.ActiveDocument != null)
            {
                //Get active document
                EnvDTE.TextDocument objTextDocument = (EnvDTE.TextDocument)_applicationObject.ActiveDocument.Object("");
                EnvDTE.TextSelection objTextSelection = objTextDocument.Selection;

                if (!String.IsNullOrEmpty(txt))
                {
                    objTextSelection.Insert(txt, (int)EnvDTE.vsInsertFlags.vsInsertFlagsContainNewText);
                    //  objTextDocument.Selection.Text = txt;
                }
            }
        }
    }
}
