namespace Apollo11.Module.Win.Controllers
{
    partial class ArbitrageVC
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
            this.Trade = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // Trade
            // 
            this.Trade.Caption = "Trade";
            this.Trade.ConfirmationMessage = null;
            this.Trade.Id = "Trade";
            this.Trade.ToolTip = null;
            this.Trade.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Trade_Execute);
            // 
            // ArbitrageVC
            // 
            this.Actions.Add(this.Trade);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction Trade;
    }
}
