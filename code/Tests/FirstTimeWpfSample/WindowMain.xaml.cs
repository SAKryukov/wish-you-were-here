/*
    FirstTime Utility Test
    
    Copyright (C) 2009-2017 by Sergey A Kryukov
    http://www.SAKryukov.org
*/
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FirstTimeWpfSample {
    using SA.Univeral.Utilities.Diagnostics;
    using Cardinal = System.UInt64;

    public partial class WindowMain : Window {
        
        public WindowMain() {
            InitializeComponent();
            this.Left = 0;
            this.Top = 0;
            this.Height = SystemParameters.MaximizedPrimaryScreenHeight - SystemParameters.ResizeFrameHorizontalBorderHeight * 2;
            this.introduction.Text = "Try to do the following repeatedly:\n\tCreate new owned window (Alt-T);\n\tClose owned window (Alt+C);\n\tCall Anonymous Method (Alt+A, see a button below);\n\tPress any key;\n\tMove mouse in and out;\n\tRe-activate this window.";
            ReportButton("Call _Anonymos Method", new Action(delegate() {
                if (FirstTime.Here)
                    MessageBox.Show("Well, it is called... only once\n\n...try again!", Title + ": Anonymous Method Call");
            }));
            buttonTestInstance.Click += (s, e) => {
                Window window = new WindowOwned();
                window.Owner = this;
                window.Show();
            }; //buttonTestInstance.Click
        } //WindowMain

        #region event reporting

        void ReportItem(string item) {
            int itemNumber = listBoxReport.Items.Add(item);
            listBoxReport.ScrollIntoView(listBoxReport.Items[itemNumber]);
        } //ReportItem
        void ReportButton(string item, Action method) {
            Button btn = new Button();
            btn.Content = item;
            btn.Margin = new Thickness(4, 2, 0, 2);
            btn.Padding = new Thickness(24, 8, 24, 8);
            btn.Click += (s, e) => { method.Invoke(); };
            int itemNumber = listBoxReport.Items.Add(btn);
            listBoxReport.ScrollIntoView(listBoxReport.Items[itemNumber]);
        } //ReportButton
        void ReportTextBlock(string item) {
            ReportTextBlock(item, Brushes.DarkOrchid, Brushes.Yellow);
        } //ReportTextBlock
        void ReportTextBlock(string item, Brush background, Brush foreground) {
            TextBlock tb = new TextBlock();
            tb.Text = item;
            tb.Background = background;
            tb.Foreground = foreground;
            if (item.Contains("\t"))
                tb.Margin = new Thickness(4, 4, 4, 4);
            else
                tb.Margin = new Thickness(1, 1, 1, 1);
            tb.Padding = new Thickness(24, 4, 24, 4);
            int itemNumber = listBoxReport.Items.Add(tb);
            listBoxReport.ScrollIntoView(listBoxReport.Items[itemNumber]);
        } //ReportTextBlock
        
        #endregion event reporting

        protected override void OnActivated(EventArgs e) {
            base.OnActivated(e);
            Cardinal activationNumber = FirstTime.Here;
            ReportTextBlock(string.Format("Activation #{0}", activationNumber));
            if (FirstTime.Here) {
                ReportItem("FIRST TIME ACTIVATION HERE!");
                WindowOwned owned = new WindowOwned();
                owned.Owner = this;
                owned.ShowInTaskbar = false;
                owned.Show();
                owned.Left = this.Left + this.Width;
            } //if
            if (FirstTime.Here)
                ReportItem("FIRST TIME call from DIFFERENT place in the activation method HERE!");
            OverloadedMethod();
            OverloadedMethod(0);
            Dispatcher.Invoke(new Action(delegate() {
                if (FirstTime.Here)
                    ReportItem("FIRST TIME Dispatcher-invoked CALL");
            }));
        } //OnActivated

        protected override void OnKeyDown(KeyEventArgs e) {
            base.OnKeyDown(e);
            if (FirstTime.Here)
                ReportItem("FIRST TIME Key Down HERE!");
        } //OnKeyDown

        protected override void OnMouseEnter(MouseEventArgs e) {
            base.OnMouseEnter(e);
            if (FirstTime.Here)
                ReportItem("FIRST TIME Mouse Entered HERE!");
        } //OnMouseEnter

        protected override void OnMouseLeave(MouseEventArgs e) {
            base.OnMouseLeave(e);
            base.OnMouseEnter(e);
            if (FirstTime.Here)
                ReportItem("FIRST TIME Mouse Leaved HERE!");
        } //OnMouseLeave

        void OverloadedMethod() {
            if (FirstTime.Here)
                ReportItem("FIRST TIME Overloaded Method HERE!");
        } //OverloadedMethod
        void OverloadedMethod(int value) {
            if (FirstTime.Here)
                ReportItem("FIRST TIME Another Overloaded Method with the Same Name HERE!");
        } //OverloadedMethod

    } //class WindowMain

} //class WindowMain
