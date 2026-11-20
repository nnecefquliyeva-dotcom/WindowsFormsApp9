
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        CourseRepository repository = new CourseRepository();

        public Form1()
        {
            InitializeComponent();

            dataGridViewCourses.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dataGridViewCourses.MultiSelect = false;

            dataGridViewCourses.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            LoadCourses();

            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;

            dataGridViewCourses.CellClick +=
                dataGridViewCourses_CellClick;

            txtCredits.KeyPress += txtCredits_KeyPress;
        }

        // Fenleri ekrana getirir
        void LoadCourses()
        {
            dataGridViewCourses.DataSource = null;

            dataGridViewCourses.DataSource =
                repository.GetAll();

            lblCount.Text =
                "Umumi fenn sayi: " + repository.Count();
        }

        // Credits ucun yalniz reqem yazilmasi
        private void txtCredits_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // Credits deyerini yoxlayir
        bool CheckCredits()
        {
            int credits;

            if (!int.TryParse(txtCredits.Text, out credits))
            {
                MessageBox.Show("Kredit daxil edin!");
                return false;
            }

            if (credits < 1 || credits > 10)
            {
                MessageBox.Show(
                    "Kredit 1 ile 10 arasinda olmalidir!");

                return false;
            }

            return true;
        }

        // Yeni fenn elave et
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtTitle.Text.Trim() == "")
            {
                MessageBox.Show("Fennin adini daxil edin!");
                return;
            }

            if (!CheckCredits())
                return;

            Course course = new Course();

            course.Title = txtTitle.Text.Trim();
            course.Credits = int.Parse(txtCredits.Text);

            repository.Add(course);

            MessageBox.Show("Fenn ugurla elave edildi!");

            ClearFields();
            LoadCourses();
        }

        // Secilmis fenni yenile
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridViewCourses.CurrentRow == null)
            {
                MessageBox.Show("Evvelce fenn secin!");
                return;
            }

            if (txtTitle.Text.Trim() == "")
            {
                MessageBox.Show("Fennin adini daxil edin!");
                return;
            }

            if (!CheckCredits())
                return;

            Course course = new Course();

            course.Id = Convert.ToInt32(
                dataGridViewCourses.CurrentRow.Cells["Id"].Value);

            course.Title = txtTitle.Text.Trim();
            course.Credits = int.Parse(txtCredits.Text);

            repository.Update(course);

            MessageBox.Show("Fenn ugurla yenilendi!");

            ClearFields();
            LoadCourses();
        }

        // Secilmis fenni sil
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewCourses.CurrentRow == null)
            {
                MessageBox.Show("Evvelce fenn secin!");
                return;
            }

            int id = Convert.ToInt32(
                dataGridViewCourses.CurrentRow.Cells["Id"].Value);

            DialogResult result = MessageBox.Show(
                "Bu fenni silmek isteyirsiniz?",
                "Silme emeliyyati",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    repository.Delete(id);

                    MessageBox.Show("Fenn silindi!");

                    ClearFields();
                    LoadCourses();
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 547)
                    {
                        MessageBox.Show(
                            "Bu fennin qiymetleri movcuddur. " +
                            "Evvelce hemin qiymetleri silmelisiniz!",
                            "Silme mumkun deyil",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        // Cedvelde fenn secildikde melumatlari TextBox-a yazir
        private void dataGridViewCourses_CellClick(
            object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtTitle.Text =
                    dataGridViewCourses.Rows[e.RowIndex]
                    .Cells["Title"].Value.ToString();

                txtCredits.Text =
                    dataGridViewCourses.Rows[e.RowIndex]
                    .Cells["Credits"].Value.ToString();
            }
        }

        // TextBox-lari temizleyir
        void ClearFields()
        {
            txtTitle.Clear();
            txtCredits.Clear();
        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {

        }
    }
}
