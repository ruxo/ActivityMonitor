using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using Drawing = System.Drawing;
using Forms = System.Windows.Forms;
using System.Collections.Generic;

namespace PAM
{
    /// <summary>
    /// Represents a thin wrapper for <see cref="Forms.NotifyIcon"/>
    /// </summary>
    [ContentProperty("Text")]
    [DefaultEvent("MouseDoubleClick")]
    public class NotificationAreaIcon : FrameworkElement
    {
        Forms.NotifyIcon _notifyIcon;

        public static readonly RoutedEvent MouseClickEvent = EventManager.RegisterRoutedEvent(
            "MouseClick", RoutingStrategy.Bubble, typeof(MouseButtonEventHandler), typeof(NotificationAreaIcon));

        public static readonly RoutedEvent MouseDoubleClickEvent = EventManager.RegisterRoutedEvent(
            "MouseDoubleClick", RoutingStrategy.Bubble, typeof(MouseButtonEventHandler), typeof(NotificationAreaIcon));

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(ImageSource), typeof(NotificationAreaIcon));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(NotificationAreaIcon));

        public static readonly DependencyProperty FormsContextMenuProperty =
            DependencyProperty.Register("MenuItems", typeof(List<Forms.MenuItem>), typeof(NotificationAreaIcon), new PropertyMetadata(new List<Forms.MenuItem>()));

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);


            // Create and initialize the window forms notify icon based
            _notifyIcon = new Forms.NotifyIcon {Text = Text};
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                _notifyIcon.Icon = FromImageSource(Icon);
            }
            _notifyIcon.Visible = FromVisibility(Visibility);

            if (MenuItems != null && MenuItems.Count > 0)
            {
                _notifyIcon.ContextMenu = new Forms.ContextMenu(MenuItems.ToArray());
            }

            _notifyIcon.MouseDown += OnMouseDown;
            _notifyIcon.MouseUp += OnMouseUp;
            _notifyIcon.MouseClick += OnMouseClick;
            _notifyIcon.MouseDoubleClick += OnMouseDoubleClick;

            Dispatcher.ShutdownStarted += OnDispatcherShutdownStarted;
        }

        private void OnDispatcherShutdownStarted(object sender, EventArgs e)
        {
            _notifyIcon.Dispose();
        }

        private void OnMouseDown(object sender, Forms.MouseEventArgs e)
        {
            OnRaiseEvent(MouseDownEvent, new MouseButtonEventArgs(
                InputManager.Current.PrimaryMouseDevice, 0, ToMouseButton(e.Button)));
        }

        private void OnMouseUp(object sender, Forms.MouseEventArgs e)
        {
            OnRaiseEvent(MouseUpEvent, new MouseButtonEventArgs(
                InputManager.Current.PrimaryMouseDevice, 0, ToMouseButton(e.Button)));
        }

        private void OnMouseDoubleClick(object sender, Forms.MouseEventArgs e)
        {
            OnRaiseEvent(MouseDoubleClickEvent, new MouseButtonEventArgs(
                InputManager.Current.PrimaryMouseDevice, 0, ToMouseButton(e.Button)));
        }

        private void OnMouseClick(object sender, Forms.MouseEventArgs e)
        {
            OnRaiseEvent(MouseClickEvent, new MouseButtonEventArgs(
                InputManager.Current.PrimaryMouseDevice, 0, ToMouseButton(e.Button)));
        }

        private void OnRaiseEvent(RoutedEvent handler, MouseButtonEventArgs e)
        {
            e.RoutedEvent = handler;
            RaiseEvent(e);
        }

        public List<Forms.MenuItem> MenuItems
        {
            get { return (List<Forms.MenuItem>)GetValue(FormsContextMenuProperty); }
            set { SetValue(FormsContextMenuProperty, value); }
        }

        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public event MouseButtonEventHandler MouseClick
        {
            add { AddHandler(MouseClickEvent, value); }
            remove { RemoveHandler(MouseClickEvent, value); }
        }

        public event MouseButtonEventHandler MouseDoubleClick
        {
            add { AddHandler(MouseDoubleClickEvent, value); }
            remove { RemoveHandler(MouseDoubleClickEvent, value); }
        }

        #region Conversion members

        private static Drawing.Icon FromImageSource(ImageSource icon)
        {
            if (icon == null)
            {
                return null;
            }
            var iconUri = new Uri(icon.ToString());
            return new Drawing.Icon(Application.GetResourceStream(iconUri).Stream);
        }

        private static bool FromVisibility(Visibility visibility)
        {
            return visibility == Visibility.Visible;
        }

        private static MouseButton ToMouseButton(Forms.MouseButtons button)
        {
            switch (button)
            {
                case Forms.MouseButtons.Left:
                    return MouseButton.Left;
                case Forms.MouseButtons.Right:
                    return MouseButton.Right;
                case Forms.MouseButtons.Middle:
                    return MouseButton.Middle;
                case Forms.MouseButtons.XButton1:
                    return MouseButton.XButton1;
                case Forms.MouseButtons.XButton2:
                    return MouseButton.XButton2;
            }
            throw new InvalidOperationException();
        }

        #endregion
    }
}