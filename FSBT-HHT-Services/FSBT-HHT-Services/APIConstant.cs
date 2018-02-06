using NetFwTypeLib;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;

namespace FSBT_HHT_Services
{
    public class APIConstant
    {
        private static APIConstant instance;

        public static APIConstant Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new APIConstant();
                }
                return instance;
            }
        }

        public string _connectionString;

        public string localIPAddress;

        public DateTime _countDate;

        public string _insert_query = "INSERT INTO tmpHHTStocktaking values(@StocktakingID, @ScanMode, @LocationCode," +
                "@Barcode, @Quantity, @NewQuantity, @Unitcode, @Flag, @Description, @SKUCode, @ExBarcode," +
                "@InBarcode, @SKUMode, @HHTName, @CountDate, @DepartmentCode, @FileName, @FlagImport," +
                "@HHTID, @CreateDate, @CreateBy, @UpdateDate, @UpdateBy, @ImportDate)";

        public string _check_query = "SELECT StocktakingID FROM tmpHHTStocktaking WHERE StocktakingID = '{0}'";

        public string _delete_query = "DELETE FROM tmpHHTStocktaking WHERE StocktakingID = '{0}'";

        public BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

        private APIConstant()
        {
            LogFile logFile = new LogFile(IniReadValueSpecial("LogFile", "LogPath"), DateTime.Now.ToString("yyyyMM") + ".log");

            SetupSQL();

            localIPAddress = IniReadValueSpecial("Setup", "IPAddress");

            SetupFirewall();
        }

        private void SetupSQL()
        {
            _connectionString = "Data Source=" + IniReadValueSpecial("Database", "DataSourceRequest") + ";" +
                "Initial Catalog=" + IniReadValueSpecial("Database", "DbNameRequest") + ";";

            if (IniReadValueSpecial("Database", "IsWindownAuthen").Equals("0"))
            {
                _connectionString += "User ID=" + IniReadValueSpecial("Database", "UserIDRequest") + ";";
                _connectionString += "Password=" + IniReadValueSpecial("Database", "PasswordRequest") + ";";
            }
            else
            {
                _connectionString += "Integrated Security=True;";
                _connectionString += "MultipleActiveResultSets=True;";
            }

            try
            {
                using (SqlConnection _con = new SqlConnection(_connectionString))
                {
                    _con.Open();
                    string query = "SELECT ValueDate FROM SystemSettings WHERE SettingKey = 'CountDate'";
                    SqlCommand cmd = new SqlCommand(query, _con);
                    cmd.CommandType = CommandType.Text;
                    _countDate = Convert.ToDateTime(cmd.ExecuteScalar());
                }
            }
            catch(Exception ex)
            {
                LogFile.write("0", ex.Message);
            }
        }

        private void SetupFirewall()
        {
            INetFwPolicy2 p = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
            try
            {
                INetFwRule2 pp = (INetFwRule2)p.Rules.Item("HHT Realtime Services");
            }
            catch (FileNotFoundException ex)
            {
                INetFwRule2 firewallRule = (INetFwRule2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
                firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
                firewallRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN;
                firewallRule.Enabled = true;
                firewallRule.Protocol = 6;
                firewallRule.LocalPorts = "15267";
                firewallRule.Name = "HHT Realtime Services";
                p.Rules.Add(firewallRule);
            }
        }

        public void Init()
        {

        }

        private string IniReadValueSpecial(string section, string Key)
        {
            bool isFoundSection = false;
            using (StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\setting.ini", Encoding.UTF8))
            {
                while (true)
                {
                    string line = sr.ReadLine();
                    if (line == null)
                    {
                        break;
                    }
                    else if (line == string.Empty)
                    {
                        continue;
                    }
                    else if (line.Substring(0, 1).Equals("[") && line.EndsWith("]"))
                    {
                        if (isFoundSection)
                        {
                            break;
                        }
                        if (line.Substring(1, line.Length - 2).Equals(section))
                        {
                            isFoundSection = true;
                        }
                        continue;
                    }
                    else
                    {
                        string[] splitWord = line.Split('=');
                        if (splitWord[0].Equals(Key))
                        {
                            return splitWord[1];
                        }
                        else
                        {
                            continue;
                        }

                    }
                }
            }
            return null;
        }


    }
}
