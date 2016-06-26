using Elasticsearch.Net;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static BlueChappie.Models.BlueChappieModels;

namespace BlueChappie
{
    class clsDataHandler
    {
        public BlueChappieSettings blueChappieSetttings = new BlueChappieSettings();
        public object GetObject(Object obj, Object Criteria)
        {

            Type typeObj = obj.GetType();
            Type filtertype = Criteria.GetType();
            if (!System.Configuration.ConfigurationManager.AppSettings["StorageType"].Equals("SQL"))
            {
                var node = new Uri(blueChappieSetttings.BlueChappieElasticServer + "/" + typeObj.Name + "lib/");
                var client = new Nest.ElasticClient(node);
                var searchRequest = new Nest.SearchRequest();
                QueryContainer andQuery = null;
                foreach (var imgFields in filtertype.GetProperties())
                {
                    if (imgFields.GetValue(Criteria) != null)
                    {
                        if (!imgFields.GetValue(Criteria).Equals(""))
                        {
                            andQuery &= new TermQuery
                            {
                                Field = imgFields,
                                Value = imgFields.GetValue(Criteria)
                            };
                        }
                    }
                }
                var query = client.Search<object>(s => s
                                       .Query(q => q
                                       .ConstantScore(c => c
                                       .Filter(fl => fl
                                        && andQuery))));
                var docs = query.Documents;
                DataTable tbl;
                foreach (var rows in docs)
                {
                    tbl = (DataTable)JsonConvert.DeserializeObject("[" + rows.Suffix("]") + "]", (typeof(DataTable)));
                    foreach (DataRow row in tbl.Rows)
                    {
                        foreach (System.Data.DataColumn fld in tbl.Columns)
                        {
                            foreach (PropertyInfo imgFields in typeObj.GetProperties())
                            {
                                if (imgFields.Name.Equals(fld.ColumnName))
                                {
                                    if (imgFields.CanWrite)
                                    {
                                        imgFields.SetValue(obj, row.Field<object>(fld.ToString()));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                System.Data.DataSet dataSet = new System.Data.DataSet();
                System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection("Database=BlueChappie;Server=localhost;uid=ProTrack2;pwd=protrack123;Connect Timeout=30;Min Pool Size=5;Max Pool Size=900;");
                System.Data.SqlClient.SqlDataAdapter sqlDataAdapter = new System.Data.SqlClient.SqlDataAdapter();
                
                string SQLStatement = "SELECT TOP 1 * FROM " + typeObj.Name + "lib with (nolock) WHERE 1=1";
                string filter = "";
                foreach (PropertyInfo imgFields in filtertype.GetProperties())
                {
                    if (imgFields.GetValue(Criteria) != null)
                    {
                        if (!imgFields.GetValue(Criteria).Equals(""))
                        {
                            filter += ((filter.Equals("")) ? "" : " AND ") + imgFields.Name + "='" + imgFields.GetValue(Criteria) + "', ";
                        }
                    }
                }
                if (filter.Length > 0)
                {
                    filter = filter.Substring(0, filter.Length - 2);
                    SQLStatement = SQLStatement.Replace("1=1", filter);
                }
                sqlDataAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand(filter, sqlConnection);
                sqlDataAdapter.Fill(dataSet);
                DataTable tbl = dataSet.Tables[0];
                tbl.TableName = typeObj.Name + "lib";
                foreach (System.Data.DataRow row in tbl.Rows)
                    foreach (System.Data.DataColumn fld in tbl.Columns)
                    {
                        foreach (PropertyInfo imgFields in typeObj.GetProperties())
                        {
                            if (imgFields.Name.Equals(fld.ColumnName))
                            {

                                if (imgFields.CanWrite)
                                {
                                    imgFields.SetValue(obj, row.Field<object>(fld.ToString()));
                                }
                            }
                        }
                    }
            }
            
            return obj;
        }

        public object GetObjectCount(Object obj, Object Criteria)
        {

            Type typeObj = obj.GetType();
            Type filtertype = Criteria.GetType();
            if (!System.Configuration.ConfigurationManager.AppSettings["StorageType"].Equals("SQL"))
            {
                var node = new Uri(blueChappieSetttings.BlueChappieElasticServer + "/" + typeObj.Name + "lib/");
                var client = new Nest.ElasticClient(node);
                var searchRequest = new Nest.SearchRequest();
                var aggregationRequest = new Nest.AggregationContainer();
                QueryContainer andQuery = null;
                ValueCountAggregation vcagg = null;
                foreach (var imgFields in filtertype.GetProperties())
                {
                    if (imgFields.GetValue(Criteria) != null)
                    {
                        if (!imgFields.GetValue(Criteria).Equals(""))
                        {
                            andQuery &= new TermQuery
                            {
                                Field = imgFields,
                                Value = imgFields.GetValue(Criteria)
                            };
                            //vcagg &= new ValueCountAggregation {
                            //    name ="a",
                            //    Field = imgFields
                            //};
                        }
                    }
                }
                var query = client.Search<object>(s => s
                                       .Query(q => q
                                       .ConstantScore(c => c
                                       .Filter(fl => fl
                                        && andQuery))));
                var docs = query.Documents;
                DataTable tbl;
                foreach (var rows in docs)
                {
                    tbl = (DataTable)JsonConvert.DeserializeObject("[" + rows.Suffix("]") + "]", (typeof(DataTable)));
                    foreach (DataRow row in tbl.Rows)
                    {
                        foreach (System.Data.DataColumn fld in tbl.Columns)
                        {
                            foreach (PropertyInfo imgFields in typeObj.GetProperties())
                            {
                                if (imgFields.Name.Equals(fld.ColumnName))
                                {
                                    if (imgFields.CanWrite)
                                    {
                                        imgFields.SetValue(obj, row.Field<object>(fld.ToString()));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                System.Data.DataSet dataSet = new System.Data.DataSet();
                System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection("Database=BlueChappie;Server=localhost;uid=ProTrack2;pwd=protrack123;Connect Timeout=30;Min Pool Size=5;Max Pool Size=900;");
                System.Data.SqlClient.SqlDataAdapter sqlDataAdapter = new System.Data.SqlClient.SqlDataAdapter();

                string SQLStatement = "SELECT TOP 1 * FROM " + typeObj.Name + "lib with (nolock) WHERE 1=1";
                string filter = "";
                foreach (PropertyInfo imgFields in filtertype.GetProperties())
                {
                    if (imgFields.GetValue(Criteria) != null)
                    {
                        if (!imgFields.GetValue(Criteria).Equals(""))
                        {
                            filter += ((filter.Equals("")) ? "" : " AND ") + imgFields.Name + "='" + imgFields.GetValue(Criteria) + "', ";
                        }
                    }
                }
                if (filter.Length > 0)
                {
                    filter = filter.Substring(0, filter.Length - 2);
                    SQLStatement = SQLStatement.Replace("1=1", filter);
                }
                sqlDataAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand(filter, sqlConnection);
                sqlDataAdapter.Fill(dataSet);
                DataTable tbl = dataSet.Tables[0];
                tbl.TableName = typeObj.Name + "lib";
                foreach (System.Data.DataRow row in tbl.Rows)
                    foreach (System.Data.DataColumn fld in tbl.Columns)
                    {
                        foreach (PropertyInfo imgFields in typeObj.GetProperties())
                        {
                            if (imgFields.Name.Equals(fld.ColumnName))
                            {

                                if (imgFields.CanWrite)
                                {
                                    imgFields.SetValue(obj, row.Field<object>(fld.ToString()));
                                }
                            }
                        }
                    }
            }

            return obj;
        }
        public void SaveObject(Object obj)
        {
            Type typeObj = obj.GetType();
            if (!System.Configuration.ConfigurationManager.AppSettings["StorageType"].Equals("SQL"))
            {
                var node = new Uri(blueChappieSetttings.BlueChappieElasticServer + "/" + typeObj.Name + "lib/");
                var config = new ConnectionConfiguration(node);
                var client = new ElasticLowLevelClient(config);
                string imgInfo = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                PostData<object> postingData = imgInfo;
                dynamic result0 = client.DoRequest<object>(HttpMethod.PUT, typeObj.Name, postingData);
            }

            else

            {
                System.Data.DataSet dataSet = new System.Data.DataSet();
                System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection("Database=BlueChappie;Server=localhost;uid=ProTrack2;pwd=protrack123;Connect Timeout=30;Min Pool Size=5;Max Pool Size=900;");
                System.Data.SqlClient.SqlDataAdapter sqlDataAdapter = new System.Data.SqlClient.SqlDataAdapter();
                sqlDataAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("SELECT TOP 0 * FROM " + typeObj.Name + "lib with (nolock)", sqlConnection);
                sqlDataAdapter.Fill(dataSet, typeObj.Name + "lib");
                System.Data.DataTable tbl = dataSet.Tables[0];
                System.Data.DataRow newRow = tbl.Rows.Add();
                foreach (System.Data.DataColumn fld in tbl.Columns)
                {
                    foreach (PropertyInfo imgFields in typeObj.GetProperties())
                    {
                        if (imgFields.Name.Equals(fld.ColumnName))
                        {
                            if (!fld.ReadOnly)
                            {
                                newRow.SetField(fld.ColumnName.ToString(), imgFields.GetValue(obj));
                            }
                        }
                    }
                }
                System.Data.DataRow dsRow = dataSet.Tables[typeObj.Name + "lib"].NewRow();
                dsRow = newRow;
                new SqlCommandBuilder(sqlDataAdapter);
                int result = sqlDataAdapter.Update(dataSet, typeObj.Name + "lib");
            }
        }
    }
}
