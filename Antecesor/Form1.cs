using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using Veecar;
using System.Management;

namespace Antecesor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Serve serve = new();

            Action<Control, Action<Control>> export = (control, action) => action(control);

            bool validated = false;
            Label[] label_valider = { label1, label2, label3, label4 };
            label_valider.forEach((it) => it.Visible = false);
            void validate(string text, string msg, int index, Predicate<string> condition)
            {
                if (condition(text))// valida la direccion ip y el puerto
                {
                    label_valider[index].Text = msg;
                    label_valider[index].Visible = true;
                    validated = false;
                }
                else
                {
                    label_valider[index].Visible = false;
                    validated = true;
                }

            }
            #region validar textbox
            TextBox[] iptext_validate = { txtiplocal, txtiphost };// para label1 y label2
            iptext_validate.forEach((txt, i) =>
            {
                txt.LostFocus += (o, e) =>
                validate(txt.Text, $"La direccion ip {txt.Text} no es valida", i, (text) => !Serve.IsIP(text.Trim()));
            });

            TextBox[] porttext_validate = { txtportlocal, txtporthost };
            porttext_validate.forEach((txt, i) =>
            {
                txt.LostFocus += (o, e) =>
                validate(txt.Text, $"El puerto {txt.Text} no es valido", i + 2,
                (text) => !(text.Trim().All(Char.IsDigit) && text.Trim().Length < 6) || string.IsNullOrWhiteSpace(text));// cada caracter no es digito y cantidad es menor a 5
            });
            #endregion

            var iplocal = serve.LocalEndPoint.split(":");
            NetworkChange.NetworkAvailabilityChanged +=
                (obj, e) =>
                {
                    serve.Reconect();
                    iplocal = serve.LocalEndPoint.split(":");
                    txtiplocal.Text = iplocal[0];
                    txtportlocal.Text = iplocal[1];

                    if (e.IsAvailable)
                        Invoke(export, this, (Action<Control>)((ctrl) => ctrl.Text = $"Reflejar servidor - Connectado a {Process.GetNetworkName()}"));
                    else
                        Invoke(export, this, (Action<Control>)((ctrl) => ctrl.Text = $"Reflejar servidor - No Connectado"));
                };
            txtiplocal.Text = iplocal[0];
            txtportlocal.Text = iplocal[1];

            button1.Click += (o, e) =>
            {
                if (validated)
                {
                    _ = serve.start(txtiphost.Text, int.Parse(txtporthost.Text));
                    button2.Enabled = true;
                    (o as Button).Enabled = false;
                }
            };
            void stopmachine()
            {
                serve.stop();
                if (button1.InvokeRequired)
                {
                    Invoke(export, button1, (Action<Control>)(ctrl => ctrl.Enabled = true));
                    Invoke(export, button2, (Action<Control>)(ctrl => ctrl.Enabled = false));
                }
                else
                {
                    button1.Enabled = true;
                    button2.Enabled = false;
                }
            };
            button2.Click += (o, e) =>
            {
                if (validated)
                    stopmachine();
            };
            serve.IfClosingOnFaulted = stopmachine;

            Pragma.OnLog += (txt) => this.Invoke(export, richTextBox1, (Action<Control>)(ctrl => ctrl.Text = txt)); ;

            //record setting
            txtiphost.Text = Properties.Settings.Default.recordiphost;
            txtporthost.Text = Properties.Settings.Default.recordporthost;
            this.FormClosing += (obj, e) =>
            {
                Properties.Settings.Default.recordiphost = txtiphost.Text;
                Properties.Settings.Default.recordporthost = txtporthost.Text;
                Properties.Settings.Default.Save();
            };
            //obtener informacion de la conexion a la red
            if (NetworkInterface.GetIsNetworkAvailable())
                Text = $"Reflejar servidor - Connectado a {Process.GetNetworkName()}";
            else
                Text = $"Reflejar servidor - No Connectado";

        }

    }
}
