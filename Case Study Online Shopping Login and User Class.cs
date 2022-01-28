using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Online_Shopping_CS
{

    public class User
    {
        public int userid;
        public string username;
        public string email;
        public string password;
        public string ship_address;
        public string bill_address;
        public int is_admin;
        public SqlConnection conn;

        public User(int userid, string username, string email,string password, string ship_add, string bill_add, int admin)
        {
            conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\sriav\Documents\SampleDB.mdf;Integrated Security=True;Connect Timeout=30");
            conn.Open();
            this.userid = userid;
            this.username = username;
            this.email = email;
            this.password = password;
            this.ship_address = ship_add;
            this.bill_address = bill_add;
            this.is_admin = admin;
        }
        public void createUserTable()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("create table Users(userid int, username varchar(20), email varchar(25), password varchar(20), shipping_address varchar(40), billing_address varchar(40), is_admin int)", this.conn);
                cmd.ExecuteNonQuery();
                Console.WriteLine("User Table Created Successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void UserRegister()
        {
            Console.WriteLine("Enter Username:");
            this.username = Console.ReadLine();

            Console.WriteLine("Enter Email:");
            this.email = Console.ReadLine();

            Console.WriteLine("Enter Password:");
            this.password = Console.ReadLine();

            Console.WriteLine("Enter Shipping Address:");
            this.ship_address = Console.ReadLine();

            Console.WriteLine("Enter Billing Address:");
            this.bill_address = Console.ReadLine();

            string query = $"insert into Users values({++this.userid},'{this.username}','{this.email}','{this.password}','{this.ship_address}','{this.bill_address}',0)";
            SqlCommand cmd = new SqlCommand(query, this.conn);

            try
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine("User Registered.......");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public User UserLogin(string username, string pw)
        {
            string query = $"select * from Users where username = '{username}' and password = '{pw}'";
            SqlCommand cmd = new SqlCommand(query, this.conn);
            SqlDataReader rd = cmd.ExecuteReader();

            if (rd.Read())
            {
                Console.WriteLine("Log In Successful!! Welcome " + rd["username"] + ".........");

                User LoogedinUser = new User((int)rd["userid"], rd["username"].ToString(), rd["email"].ToString(), rd["password"].ToString(), rd["shipping_address"].ToString(), rd["billing_address"].ToString(),(int)rd["is_admin"]);
                rd.Close();
                return LoogedinUser;
            }
            else
            {
                Console.WriteLine("Username or Password entered is incorrect! Try Again");
                rd.Close();
                return null;
            }
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            int ch = 0;
            User user1 = new User(0,"Admin","admin@gmail.com","admin123","admin_house","admin_home",1);
            user1.createUserTable();

            while (ch < 3)
            {
                Console.WriteLine("\n\nEnter 1 to Register new user\nEnter 2 for User Login\nEnter 3 to Exit");
                ch = Convert.ToInt32(Console.ReadLine());
                switch (ch)
                {
                    case 1:
                        {
                            user1.UserRegister();
                        }
                        break;
                    case 2:
                        {
                            string username, pw;
                            Console.WriteLine("\n\nEnter Username:");
                            username = Console.ReadLine();
                            Console.WriteLine("\nEnter Password:");
                            pw = Console.ReadLine();
                            try
                            {
                                User newuser = user1.UserLogin(username, pw);
                                if (newuser != null)
                                Console.WriteLine("Logged in User's Name is:" + newuser.username.ToString());
                            }
                            catch (Exception ex)
                            {
                                Console.Write(ex.Message);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}