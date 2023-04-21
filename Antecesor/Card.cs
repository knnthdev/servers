using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Linq;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;
using Veecar;

namespace Antecesor
{
    public partial class Card : UserControl, IMaterialControl
    {
        private Control _oldParent;
        private bool _shadowDrawEventSubscribed;
        private Timer timer;
        int _index = 0;
        int pos_y_previus = 0;
        TransHandler pos;

        public Card()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, value: true);
            base.Paint += paintControl;
            InitializeComponent();
            BackColor = SkinManager.BackgroundColor;
            ForeColor = SkinManager.TextHighEmphasisColor;
            base.Margin = new Padding(SkinManager.FORM_PADDING);
            base.Padding = new Padding(SkinManager.FORM_PADDING);
            Anchor = AnchorStyles.Left | AnchorStyles.Right;
            errorProvider1.Icon = new(Assembly.GetExecutingAssembly().GetManifestResourceStream("Antecesor.advertencia.ico"));

            timer = new()
            {
                Enabled = false,
                Interval = 10
            };

            //controlar position
            pos = new TransHandler(SkinManager.FORM_PADDING);

            timer.Tick += (o, e) =>
            {
                Location = new(Location.X, pos.Put);
                if (pos.Completed)
                    timer.Stop();
            };

            textBox2.CausesValidation = false;

            textBox1.Validating += (o, e) => validateIP();
            textBox2.Validating += (o, e) => validatePort();

            ParentChanged += (o, e) =>
            {
                errorProvider1.ContainerControl = ParentForm;
            };
        }

        #region Custom

        public string AddressLabel { get => iptext.Text; set => iptext.Text = value; }
        public string TextboxIp { get => textBox1.Text; set => textBox1.Text = value; }
        public int TextboxPort { get => int.Parse(textBox2.Text); set => textBox2.Text = value.ToString(); }
        public int Index { get => _index; set => IndexChanged(value); }

        void IndexChanged(int index)
        {
            var min_top = (_index * Size.Height) + SkinManager.FORM_PADDING;
            pos_y_previus = Location.Y;
            _index = index;
            pos = new(min_top);
            pos.SetPos(pos_y_previus);
            timer.Start();
        }
        bool validateIP()
        {
            var ip = textBox1.Text;
            if (!Serve.IsIP(ip))
            {
                textBox1.CausesValidation = false;
                errorProvider1.SetError(textBox1, $"{ip} no es valido");
                errorProvider1.SetIconAlignment(textBox1, ErrorIconAlignment.BottomLeft);
                return false;
            }
            errorProvider1.Clear();
            return true;
        }
        bool validatePort()
        {
            var port = textBox2.Text;
            if (port.filter(char.IsDigit).Count() != port.Length)
            {
                if (port.Length > 5)
                    errorProvider1.SetError(textBox2, $"{port} Puerto demasiado grande");
                else
                    errorProvider1.SetError(textBox2, $"{port} Puerto no valido");
                errorProvider1.SetIconAlignment(textBox2, ErrorIconAlignment.BottomLeft);
                return false;
            }
            errorProvider1.Clear();
            return true;
        }
        public bool ValidateData()
        {
            var ip = textBox1.Text;
            var port = textBox2.Text;
            if (string.IsNullOrWhiteSpace(ip) || string.IsNullOrWhiteSpace(port))
                return false;
            return validateIP() && validatePort();
        }

        public void Unablebutton(string name, bool enable)
        {
            switch (name.ToLowerInvariant())
            {
                case "iniciar":
                    btn1.Enabled = !enable; break;
                case "parar":
                    btn2.Enabled = !enable; break;
                case "eliminar":
                    btn3.Enabled = !enable; break;
                default:
                    throw new ArgumentException(nameof(name),
                        "No existe ningun boton con ese nombre, verifique que el valor sea el correcto");
            }
        }
        public void OnClick(string name, Action<MouseEventArgs, int> action)
        {
            switch (name.ToLowerInvariant())
            {
                case "iniciar":
                    btn1.MouseClick += (o, e) => action(e, _index); break;
                case "parar":
                    btn2.MouseClick += (o, e) => action(e, _index); break;
                case "eliminar":
                    btn3.MouseClick += (o, e) => action(e, _index); break;
                default:
                    throw new ArgumentException(nameof(name),
                        "No existe ningun boton con ese nombre, verifique que el valor sea el correcto");
            }
        }


        #endregion

        #region Default

        [Browsable(false)]
        public int MinTop { get; set; }

        [Browsable(false)]
        public int Depth { get; set; }

        [Browsable(false)]
        public MaterialSkinManager SkinManager => MaterialSkinManager.Instance;

        [Browsable(false)]
        public MouseState MouseState { get; set; }

        private void drawShadowOnParent(object sender, PaintEventArgs e)
        {
            if (base.Parent == null)
            {
                RemoveShadowPaintEvent((Control)sender, drawShadowOnParent);
                return;
            }

            Graphics graphics = e.Graphics;
            Rectangle bounds = new Rectangle(base.Location, base.ClientRectangle.Size);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            DrawUtils.DrawSquareShadow(graphics, bounds);
        }

        protected override void InitLayout()
        {
            base.LocationChanged += delegate
            {
                base.Parent?.Invalidate();
            };
            ForeColor = SkinManager.TextHighEmphasisColor;
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (base.Parent != null)
            {
                AddShadowPaintEvent(base.Parent, drawShadowOnParent);
            }

            if (_oldParent != null)
            {
                RemoveShadowPaintEvent(_oldParent, drawShadowOnParent);
            }

            _oldParent = base.Parent;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (base.Parent != null)
            {
                if (base.Visible)
                {
                    AddShadowPaintEvent(base.Parent, drawShadowOnParent);
                }
                else
                {
                    RemoveShadowPaintEvent(base.Parent, drawShadowOnParent);
                }
            }
        }

        private void AddShadowPaintEvent(Control control, PaintEventHandler shadowPaintEvent)
        {
            if (!_shadowDrawEventSubscribed)
            {
                control.Paint += shadowPaintEvent;
                control.Invalidate();
                _shadowDrawEventSubscribed = true;
            }
        }

        private void RemoveShadowPaintEvent(Control control, PaintEventHandler shadowPaintEvent)
        {
            if (_shadowDrawEventSubscribed)
            {
                control.Paint -= shadowPaintEvent;
                control.Invalidate();
                _shadowDrawEventSubscribed = false;
            }
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            BackColor = SkinManager.BackgroundColor;
        }

        private void paintControl(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.Clear(base.Parent.BackColor);
            RectangleF rect = new RectangleF(base.ClientRectangle.Location, base.ClientRectangle.Size);
            rect.X -= 0.5f;
            rect.Y -= 0.5f;
            GraphicsPath path = DrawUtils.CreateRoundRect(rect, 4f);
            DrawUtils.DrawSquareShadow(graphics, base.ClientRectangle);
            using SolidBrush brush = new SolidBrush(BackColor);
            graphics.FillPath(brush, path);
        }
        #endregion
    }
}
