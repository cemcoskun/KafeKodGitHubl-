﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KafeKod.Data;

namespace KafeKodGitHublı
{
    public partial class Urunler : Form
    {
        KafeContext db;
        public Urunler(KafeContext kafeVeri)
        {
            db = kafeVeri;
            InitializeComponent();
            dgvUrunler.AutoGenerateColumns = false;
            dgvUrunler.DataSource = new BindingSource(db.Urunler.OrderBy(x => x.UrunAd).ToList(), null);
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            string urunAd = txtUrunAd.Text.Trim();
            if (urunAd == "")
            {
                MessageBox.Show("Lütfen bir ürün adı giriniz.");
                return;
            }
            db.Urunler.Add(new Urun
            {
                UrunAd = urunAd,
                BirimFiyat = nudBirimFiyat.Value
            });
            db.SaveChanges();
            dgvUrunler.DataSource = new BindingSource(db.Urunler.OrderBy(x => x.UrunAd).ToList(), null);
        }

        private void dgvUrunler_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Geçersiz bir değer girdiniz.");
        }

        private void dgvUrunler_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                if (e.FormattedValue.ToString().Trim() == "")
                {
                    dgvUrunler.Rows[e.RowIndex].ErrorText = "Ürün ad boş geçilemez.";
                    e.Cancel = true;
                }
                else
                {
                    dgvUrunler.Rows[e.RowIndex].ErrorText = "";
                }
            }
            db.SaveChanges();
        }

        private void dgvUrunler_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            Urun urun = (Urun)e.Row.DataBoundItem;
            if (urun.SiparisDetaylar.Count > 0)
            {
                MessageBox.Show("Bu ürün geçmiş siparişlerle ilişkili olduğu için silinemez.");
                e.Cancel = true;
                return;
            }
            db.Urunler.Remove(urun);
            db.SaveChanges();
        }

        private void Urunler_FormClosed(object sender, FormClosedEventArgs e)
        {
            txtUrunAd.Focus();
        }
    }
}
