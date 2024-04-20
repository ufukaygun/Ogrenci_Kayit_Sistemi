using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ogrenci_Kayit_Sistemi
{
    public partial class Form1 : Form
    {
        SqlConnection _baglanti = new SqlConnection("Server=.;Database=DbNotKayit;User Id=sa;Password=1234");

        public Form1()
        {
            InitializeComponent();
            OgrListele();
        }

        void OgrListele()
        {
            dataGridView1.Rows.Clear();
             _baglanti.Open();

            SqlCommand cmd = new SqlCommand("select * from Ders", _baglanti);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                dataGridView1.Rows.Add(dr[1], dr[2], dr[3], dr[4], dr[5], dr[6], dr[7], dr[8]);
            }


            _baglanti.Close();
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            _baglanti.Open();

            SqlCommand cmd = new SqlCommand("insert Ders(OgrNo,Ad,Soyad) Values(@p1,@p2,@p3)", _baglanti);
            cmd.Parameters.AddWithValue("@p1", mskNumara.Text);
            cmd.Parameters.AddWithValue("@p2", txtAd.Text);
            cmd.Parameters.AddWithValue("@p3", txtSoyad.Text);

            cmd.ExecuteNonQuery();
            MessageBox.Show("Öğrenci Ekleme Başarılı");
            _baglanti.Close();

            mskNumara.Clear();
            txtAd.Clear();
            txtSoyad.Clear();

            OgrListele();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;

            mskNumara.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
            txtAd.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            txtSoyad.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
            txtSinav1.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
            txtSinav2.Text = dataGridView1.Rows[secilen].Cells[4].Value.ToString();
            txtSinav3.Text = dataGridView1.Rows[secilen].Cells[5].Value.ToString();

        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            double sinav1, sinav2, sinav3, ortalama;

            sinav1 = Convert.ToDouble(txtSinav1.Text);
            sinav2 = double.Parse(txtSinav2.Text);
            sinav3 = double.Parse(txtSinav3.Text);

            ortalama = (sinav1 + sinav2 + sinav3) / 3;

            string durum = ortalama >= 50 ? "True" : "False";

            _baglanti.Open();

            SqlCommand sql = new SqlCommand("update Ders set sinav1=@p1,sinav2=@p2,sinav3=@p3,ortalama=@p4,durum=@p5 where OgrNo=@p6",_baglanti);

            sql.Parameters.AddWithValue("@p1", sinav1);
            sql.Parameters.AddWithValue("@p2", sinav2);
            sql.Parameters.AddWithValue("@p3", sinav3);
            sql.Parameters.AddWithValue("@p4", Convert.ToDecimal(ortalama));
            sql.Parameters.AddWithValue("@p5", durum);
            sql.Parameters.AddWithValue("@p6", mskNumara.Text);
            sql.ExecuteNonQuery();

            _baglanti.Close();
            OgrListele();
        }
    }
}
