using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using Veecar;

namespace Antecesor
{
    public partial class Form2 : MaterialForm
    {
        public Form2()
        {
            InitializeComponent();
            var serve = new Serve();
            var manager = MaterialSkinManager.Instance;
            manager.AddFormToManage(this);
            manager.Theme = MaterialSkinManager.Themes.LIGHT;
            //manager.ColorScheme = new(Color.Red, Color.IndianRed, Color.LightSalmon, Color.Olive, TextShade.WHITE); ;
            manager.ColorScheme = new(Color.FromArgb(45, 47, 48), Color.FromArgb(32, 32, 35), Color.LightSalmon, Color.Olive, TextShade.WHITE); ;

            List<(int index, Serve serve, Card card)> collection = new();
            int index = 0;

            btnAppend.Click += (o, e) =>
            {
                Serve serve = new();
                Card card = new()
                {
                    AddressLabel = serve.LocalEndPoint,
                    Index = index,
                    Name = $"card_{index}_",
                    Width = PnCards.Width
                };
                card.Unablebutton("parar", true);
                card.OnClick("iniciar", (e, i) =>
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        if (collection[i].card.ValidateData())
                        {
                            _ = collection[i].serve.start(collection[i].card.TextboxIp, collection[i].card.TextboxPort);
                            collection[i].card.Unablebutton("iniciar", true);
                            collection[i].card.Unablebutton("parar", false);
                        }
                    }
                });
                card.OnClick("parar", (e, i) =>
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        collection[i].serve.stop();
                        collection[i].card.Unablebutton("parar", true);
                        collection[i].card.Unablebutton("iniciar", false);
                    }
                });
                card.OnClick("eliminar", (e, _index) =>
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        collection[_index].serve.stop();
                        collection.RemoveAt(_index);
                        collection.forEach((it, i) =>
                        {
                            it.index = i;
                            it.card.Index = it.index;
                        });
                        tabPage1.Controls.Remove(collection[_index].card);
                    }
                });

                collection.Add((index, serve, card));
                this.PnCards.Controls.Add(card);
            };

        }

        private void InitializeComponent()
        {
            this.materialTabSelector1 = new MaterialSkin.Controls.MaterialTabSelector();
            this.materialTabControl1 = new MaterialSkin.Controls.MaterialTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.materialCard1 = new MaterialSkin.Controls.MaterialCard();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.btnAppend = new MaterialSkin.Controls.MaterialButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.PnCards = new MaterialSkin.Controls.MaterialCard();
            this.materialTabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.materialCard1.SuspendLayout();
            this.SuspendLayout();
            // 
            // materialTabSelector1
            // 
            this.materialTabSelector1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.materialTabSelector1.BaseTabControl = this.materialTabControl1;
            this.materialTabSelector1.CharacterCasing = MaterialSkin.Controls.MaterialTabSelector.CustomCharacterCasing.Normal;
            this.materialTabSelector1.Depth = 0;
            this.materialTabSelector1.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialTabSelector1.Location = new System.Drawing.Point(0, 56);
            this.materialTabSelector1.Margin = new System.Windows.Forms.Padding(0);
            this.materialTabSelector1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialTabSelector1.Name = "materialTabSelector1";
            this.materialTabSelector1.Size = new System.Drawing.Size(638, 19);
            this.materialTabSelector1.TabIndex = 0;
            this.materialTabSelector1.Text = "materialTabSelector1";
            // 
            // materialTabControl1
            // 
            this.materialTabControl1.Controls.Add(this.tabPage1);
            this.materialTabControl1.Controls.Add(this.tabPage2);
            this.materialTabControl1.Depth = 0;
            this.materialTabControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.materialTabControl1.Location = new System.Drawing.Point(3, 78);
            this.materialTabControl1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialTabControl1.Multiline = true;
            this.materialTabControl1.Name = "materialTabControl1";
            this.materialTabControl1.SelectedIndex = 0;
            this.materialTabControl1.Size = new System.Drawing.Size(632, 389);
            this.materialTabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.PnCards);
            this.tabPage1.Controls.Add(this.materialCard1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(624, 363);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Servidores";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // materialCard1
            // 
            this.materialCard1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.materialCard1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialCard1.Controls.Add(this.materialLabel1);
            this.materialCard1.Controls.Add(this.btnAppend);
            this.materialCard1.Depth = 0;
            this.materialCard1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialCard1.Location = new System.Drawing.Point(0, 0);
            this.materialCard1.Margin = new System.Windows.Forms.Padding(14);
            this.materialCard1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCard1.Name = "materialCard1";
            this.materialCard1.Padding = new System.Windows.Forms.Padding(14);
            this.materialCard1.Size = new System.Drawing.Size(624, 48);
            this.materialCard1.TabIndex = 3;
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel1.ForeColor = System.Drawing.Color.White;
            this.materialLabel1.Location = new System.Drawing.Point(17, 16);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(133, 19);
            this.materialLabel1.TabIndex = 1;
            this.materialLabel1.Text = "Reflejar Servidores";
            // 
            // btnAppend
            // 
            this.btnAppend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAppend.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAppend.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Dense;
            this.btnAppend.Depth = 0;
            this.btnAppend.HighEmphasis = true;
            this.btnAppend.Icon = null;
            this.btnAppend.Location = new System.Drawing.Point(556, 6);
            this.btnAppend.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnAppend.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnAppend.Name = "btnAppend";
            this.btnAppend.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnAppend.Size = new System.Drawing.Size(64, 36);
            this.btnAppend.TabIndex = 2;
            this.btnAppend.Text = "+";
            this.btnAppend.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnAppend.UseAccentColor = false;
            this.btnAppend.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(624, 363);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Información de Red";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // PnCards
            // 
            this.PnCards.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PnCards.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.PnCards.Depth = 0;
            this.PnCards.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.PnCards.Location = new System.Drawing.Point(0, 49);
            this.PnCards.Margin = new System.Windows.Forms.Padding(14);
            this.PnCards.MouseState = MaterialSkin.MouseState.HOVER;
            this.PnCards.Name = "PnCards";
            this.PnCards.Padding = new System.Windows.Forms.Padding(14);
            this.PnCards.Size = new System.Drawing.Size(624, 314);
            this.PnCards.TabIndex = 4;
            // 
            // Form2
            // 
            this.ClientSize = new System.Drawing.Size(638, 470);
            this.Controls.Add(this.materialTabSelector1);
            this.Controls.Add(this.materialTabControl1);
            this.DrawerUseColors = true;
            this.Name = "Form2";
            this.Text = "Antecesor";
            this.materialTabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.materialCard1.ResumeLayout(false);
            this.materialCard1.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}

