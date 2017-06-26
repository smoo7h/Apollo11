namespace Apollo11.Module.Win.Controllers
{
    partial class CurrentSellVC
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ToggleLiveData = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ToggleLiveData
            // 
            this.ToggleLiveData.Caption = "Toggle Live Data";
            this.ToggleLiveData.ConfirmationMessage = null;
            this.ToggleLiveData.Id = "ToggleLiveData";
            this.ToggleLiveData.ToolTip = null;
            this.ToggleLiveData.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleAction1_Execute);
            // 
            // CurrentSellVC
            // 
            this.Actions.Add(this.ToggleLiveData);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ToggleLiveData;
    }
}
