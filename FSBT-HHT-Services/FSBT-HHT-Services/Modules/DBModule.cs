using FSBT_HHT_Services.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace FSBT_HHT_Services.Modules
{
    public class DBModule
    {

        public DBModule()
        {

        }

        public List<Stocktaking> GetAllStocktaking()
        {
            try
            {
                using (SqlConnection _con = new SqlConnection(APIConstant.Instance._connectionString))
                {
                    _con.Open();
                    string query = "SELECT TOP 5 * FROM tmpHHTStocktaking";
                    SqlCommand cmd = new SqlCommand(query, _con);
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Stocktaking> returnList = new List<Stocktaking>();
                    while (reader.Read())
                    {
                        Stocktaking temp = new Stocktaking();
                        temp.StocktakingID = reader.IsDBNull(0) ? null : reader.GetString(0);
                        temp.ScanMode = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                        temp.LocationCode = reader.IsDBNull(2) ? null : reader.GetString(2);
                        temp.Barcode = reader.IsDBNull(3) ? null : reader.GetString(3);
                        temp.Quantity = reader.IsDBNull(4) ? 0 : reader.GetDecimal(4);
                        temp.UnitCode = reader.IsDBNull(6) ? 0 : reader.GetInt32(6);
                        temp.Flag = reader.IsDBNull(7) ? null : reader.GetString(7);
                        temp.Description = reader.IsDBNull(8) ? null : reader.GetString(8);
                        temp.SKUCode = reader.IsDBNull(9) ? null : reader.GetString(9);
                        temp.ExBarcode = reader.IsDBNull(10) ? null : reader.GetString(10);
                        temp.InBarcode = reader.IsDBNull(11) ? null : reader.GetString(11);
                        temp.SKUMode = reader.IsDBNull(12) ? false : reader.GetBoolean(12);
                        temp.HHTName = reader.IsDBNull(13) ? null : reader.GetString(13);
                        temp.DepartmentCode = reader.IsDBNull(15) ? null : reader.GetString(15);

                        returnList.Add(temp);
                    }
                    return returnList;
                }
            }
            catch (Exception ex)
            {
                LogFile.write("0", ex.Message);
                return null;
            }
        }

        public bool IsRemainInDB(string stocktakingID, SqlConnection _con)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(string.Format(APIConstant.Instance._check_query, stocktakingID), _con);
                cmd.CommandType = CommandType.Text;
                object scalar = cmd.ExecuteScalar();
                if (scalar != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogFile.write("0", ex.Message);
                return false;
            }


        }

        public bool InsertStocktaking(Stocktaking temp)
        {
            try
            {
                using (SqlConnection _con = new SqlConnection(APIConstant.Instance._connectionString))
                {
                    _con.Open();

                    SqlCommand _insert_command = new SqlCommand(APIConstant.Instance._insert_query, _con);
                    _insert_command.CommandType = CommandType.Text;

                    DateTime currentDT = DateTime.Now;

                    _insert_command.Parameters.Clear();
                    foreach (PropertyInfo propertyInfo in temp.GetType().GetProperties(APIConstant.Instance.flags))
                    {
                        if (propertyInfo.GetValue(temp, null) == null)
                        {
                            _insert_command.Parameters.AddWithValue("@" + propertyInfo.Name, DBNull.Value);
                        }
                        else
                        {
                            _insert_command.Parameters.AddWithValue("@" + propertyInfo.Name, propertyInfo.GetValue(temp, null));
                        }
                    }

                    //Random rand = new Random();
                    //string uniqueID = currentDT.ToString("yy") + currentDT.ToString("MM") + currentDT.ToString("dd") + "999" + rand.Next(0, 10000000).ToString().PadLeft(7, '0');

                    //_insert_command.Parameters.AddWithValue("@StocktakingID", temp.StocktakingID);
                    //_insert_command.Parameters.AddWithValue("@ScanMode", temp.ScanMode);
                    //_insert_command.Parameters.AddWithValue("@LocationCode", temp.LocationCode);
                    //_insert_command.Parameters.AddWithValue("@Barcode", temp.Barcode);
                    //_insert_command.Parameters.AddWithValue("@Quantity", temp.Quantity);
                    _insert_command.Parameters.AddWithValue("@NewQuantity", DBNull.Value);
                    //_insert_command.Parameters.AddWithValue("@Unitcode", temp.UnitCode);
                    //_insert_command.Parameters.AddWithValue("@Flag", temp.Flag);
                    //_insert_command.Parameters.AddWithValue("@Description", temp.Description);
                    //_insert_command.Parameters.AddWithValue("@SKUCode", temp.SKUCode);
                    //_insert_command.Parameters.AddWithValue("@ExBarcode", temp.ExBarcode);
                    //_insert_command.Parameters.AddWithValue("@InBarcode", temp.InBarcode);
                    //_insert_command.Parameters.AddWithValue("@SKUMode", temp.SKUMode);
                    //_insert_command.Parameters.AddWithValue("@HHTName", temp.HHTName);
                    _insert_command.Parameters.AddWithValue("@CountDate", APIConstant.Instance._countDate);
                    //_insert_command.Parameters.AddWithValue("@DepartmentCode", temp.DepartmentCode);
                    _insert_command.Parameters.AddWithValue("@FileName", "");
                    _insert_command.Parameters.AddWithValue("@FlagImport", false);
                    //_insert_command.Parameters.AddWithValue("@HHTID", temp.HHTID);
                    _insert_command.Parameters.AddWithValue("@CreateDate", currentDT);
                    //_insert_command.Parameters.AddWithValue("@CreateBy", temp.Username);
                    _insert_command.Parameters.AddWithValue("@UpdateDate", currentDT);
                    //_insert_command.Parameters.AddWithValue("@UpdateBy", temp.Username);
                    _insert_command.Parameters.AddWithValue("@ImportDate", currentDT);

                    _insert_command.ExecuteNonQuery();

                    return true;
                }

            }
            catch (Exception ex)
            {
                LogFile.write("0", ex.Message);
                return false;
            }

        }

        public void DeleteStocktaking(string stocktakingID)
        {
            try
            {
                using (SqlConnection _con = new SqlConnection(APIConstant.Instance._connectionString))
                {
                    _con.Open();
                    SqlCommand cmd = new SqlCommand(string.Format(APIConstant.Instance._delete_query, stocktakingID), _con);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                LogFile.write("0", ex.Message);
            }
        }

        public bool UpdateStocktaking(Stocktaking temp)
        {
            try
            {
                using (SqlConnection _con = new SqlConnection(APIConstant.Instance._connectionString))
                {
                    _con.Open();
                    if (IsRemainInDB(temp.StocktakingID, _con))
                    {
                        DeleteStocktaking(temp.StocktakingID);
                        InsertStocktaking(temp);
                        return true;
                    }
                    else
                    {
                        InsertStocktaking(temp);
                        return true;
                    }
                }

            }
            catch (Exception ex)
            {
                LogFile.write("0", ex.Message);
                return false;
            }

        }
    }
}
