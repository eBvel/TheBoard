using DBModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp6
{
    public partial class Board : Form
    {
        private static short minute = 60;
        private string runStringText;

        public Board()
        {
            InitializeComponent();
            runStringText = "Путин заявил, что Россия «зубы выбьет» всем, кто попробует что-то у нее «откусить»!" +
                "     У России самые современные силы ядерного сдерживания из всех держав!" +
                "     Глава государства также отметил, в России на национальную оборону в этом году выделяется три триллиона 116 миллиардов рублей." +
                "     По словам президента, для России важны траты в процентном отношении." +
                "     Президент РФ Владимир Путин в четверг на заседании оргкомитета \"Победа\" пообещал сделать все необходимое, чтобы усилить и упрочнить нашу страну.";
            label3.Text = runStringText;
        }

        private async void LoadDate()
        {
            var tripsList = await Task.Run(() => GetTripsList());
            dataGridView1.DataSource = tripsList.Where(trip => trip.DepartureDate.ToShortDateString() == DateTime.Today.ToShortDateString()
                                      && Convert.ToDateTime(trip.Time) >= DateTime.Now).ToList();
        }

        private async Task<List<Trip>> GetTripsList()
        {
            using (var db = new dbContext())
            {
                await db.Stations.LoadAsync();
                await db.Routes.LoadAsync();
                await db.TimetableRoutes.LoadAsync();
                await db.Trains.LoadAsync();
                await db.Trips.LoadAsync();

                return await db.Trips.ToListAsync();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox4.Text = DateTime.Now.ToLongTimeString();
            textBox2.Text = DateTime.Now.ToShortDateString();

            if (minute == 60)
            {
                LoadDate();
                minute = 0;
            }
            else minute++;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (label3.Left > -label3.Width+10)
            {
                label3.Left -= 1;
            }
            else
            {
                label3.Left = runString.Width;
            }
        }
    }
}
