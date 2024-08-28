using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using Apparent.Model;
using System.Threading.Tasks;
using System.Web.DynamicData;
using System.Web.Routing;

namespace Apparent.DBContext
{
	public class ProductDBContext
	{
		private string Cs = ConfigurationManager.ConnectionStrings["abcd"].ConnectionString;

		public DataTable ProductAdd(ProductMaster model)
		{
			try
			{

				DataTable dt = new DataTable();
				SqlConnection con = new SqlConnection(Cs);
				SqlCommand cmd = new SqlCommand("Sp_Product", con);
				cmd.CommandType = CommandType.StoredProcedure;
				con.Open();
				cmd.Parameters.AddWithValue("@CompanyId", model.CompanyId);
				cmd.Parameters.AddWithValue("@CompanyName", model.CompanyName);
				cmd.Parameters.AddWithValue("@CompanyEmail", model.CompanyEmail);
				cmd.Parameters.AddWithValue("@Phone", model.Phone);
				cmd.Parameters.AddWithValue("@CompanyWebsite", model.CompanyWebsite);
				cmd.Parameters.AddWithValue("@Twitter", model.Twitter);
				cmd.Parameters.AddWithValue("@LinkedIn", model.LinkedIn);
				//cmd.Parameters.AddWithValue("@HQLocation", model.HQLocation);
				cmd.Parameters.AddWithValue("@Comment", model.Notes);
				cmd.Parameters.AddWithValue("@ProductIcon", model.ProductIcon);
				cmd.Parameters.AddWithValue("@Images", model.Images);
				cmd.Parameters.AddWithValue("@ProductName", model.ProductName);
				cmd.Parameters.AddWithValue("@Description", model.Description);
				cmd.Parameters.AddWithValue("@Category", model.Category);
				cmd.Parameters.AddWithValue("@AboutProduct", model.AboutProduct);


				cmd.Parameters.AddWithValue("@Website", model.Website);
				cmd.Parameters.AddWithValue("@SupportLanguages", model.SupportLanguages);
				//cmd.Parameters.AddWithValue("@PdfFileList", model.PdfFileList);
				cmd.Parameters.AddWithValue("@ImageList", model.ImagesList);
				cmd.Parameters.AddWithValue("@ProductVideo", model.VideoList);
				cmd.Parameters.AddWithValue("@Task", "AddProduct");

				SqlDataAdapter sda = new SqlDataAdapter(cmd);
				sda.Fill(dt);
				con.Close();
				return dt;



			}





			catch
			{
				return null;
			}

		}


		public DataSet sellerProductList(string Sellerid)
		{
			try
			{
				DataSet ds = new DataSet();
				SqlConnection con = new SqlConnection(Cs);
				SqlCommand cmd = new SqlCommand("Sp_Product", con);
				cmd.CommandType = CommandType.StoredProcedure;
				con.Open();

				cmd.Parameters.AddWithValue("@SellerId", Sellerid);
				cmd.Parameters.AddWithValue("@Task", "SellerProduct");

				SqlDataAdapter sda = new SqlDataAdapter(cmd);
				sda.Fill(ds);
				con.Close();
				return ds;
			}
			catch
			{
				return null;
			}

		}
		public DataSet ProductList()
		{
			try
			{
				DataSet ds = new DataSet();
				SqlConnection con = new SqlConnection(Cs);
				SqlCommand cmd = new SqlCommand("Sp_Product", con);
				cmd.CommandType = CommandType.StoredProcedure;
				con.Open();


				cmd.Parameters.AddWithValue("@Task", "ShowProduct");

				SqlDataAdapter sda = new SqlDataAdapter(cmd);
				sda.Fill(ds);
				con.Close();
				return ds;
			}
			catch
			{
				return null;
			}

		}
		public DataSet ProducDetails(string ProductId)
		{
			try
			{
				DataSet ds = new DataSet();
				SqlConnection con = new SqlConnection(Cs);
				SqlCommand cmd = new SqlCommand("Sp_Product", con);
				cmd.CommandType = CommandType.StoredProcedure;
				con.Open();

				cmd.Parameters.AddWithValue("@ProductId", ProductId);
				cmd.Parameters.AddWithValue("@Task", "ProductDetails");

				SqlDataAdapter sda = new SqlDataAdapter(cmd);
				sda.Fill(ds);
				con.Close();
				return ds;
			}
			catch
			{
				return null;
			}

		}

		public DataTable ProductPrice(ProductMaster model)
		{
			try
			{

				DataTable dt = new DataTable();
				SqlConnection con = new SqlConnection(Cs);
				SqlCommand cmd = new SqlCommand("Sp_Product", con);
				cmd.CommandType = CommandType.StoredProcedure;
				con.Open();
				cmd.Parameters.AddWithValue("@CompanyId", model.CompanyId);
				cmd.Parameters.AddWithValue("@ProductId", model.id);
				cmd.Parameters.AddWithValue("@Mergin", model.Mergin);
				cmd.Parameters.AddWithValue("@Packeginglist", model.Packeglist);
				//cmd.Parameters.AddWithValue("@TokenPackegList", model.Token_List);
				cmd.Parameters.AddWithValue("@Product_Url", model.Product_Url);
				cmd.Parameters.AddWithValue("@Task", "ProductPrice");
				SqlDataAdapter sda = new SqlDataAdapter(cmd);
				sda.Fill(dt);
				con.Close();
				return dt;
			}
			catch
			{
				return null;
			}
		}
		public DataTable AddFeatures(features model)
		{
			try
			{
				DataTable dt1 = new DataTable();
				SqlConnection con = new SqlConnection(Cs);
				SqlCommand cmd = new SqlCommand("Sp_Product", con);
				cmd.CommandType = CommandType.StoredProcedure;
				con.Open();
				cmd.Parameters.AddWithValue("@ProductId", model.ProductId);
				cmd.Parameters.AddWithValue("@FeaturesGroup3", model.FeaturesGroup3);
				cmd.Parameters.AddWithValue("@FeaturesGroup2", model.FeaturesGroup2);
				cmd.Parameters.AddWithValue("@FeaturesGroup1", model.FeaturesGroup1);
				cmd.Parameters.AddWithValue("@Add_featuresg1", model.Add_featuresg1);
				cmd.Parameters.AddWithValue("@Add_featuresg2", model.Add_featuresg2);
				cmd.Parameters.AddWithValue("@Add_featuresg3", model.Add_featuresg3);
				cmd.Parameters.AddWithValue("@CompanyId", model.CompanyId);
				cmd.Parameters.AddWithValue("@Comment", model.Features_Commentgroup);
				cmd.Parameters.AddWithValue("@Task", "AddFeatures");
				SqlDataAdapter sda = new SqlDataAdapter(cmd);
				sda.Fill(dt1);
				con.Close();
				return dt1;
			}
			catch
			{
				return null;
			}
		}

		public DataTable SelectProductbyCategory(string ProductCategory)
		{
			try
			{
				DataTable dt1 = new DataTable();
				SqlConnection con = new SqlConnection(Cs);
				SqlCommand cmd = new SqlCommand("Sp_Product", con);
				cmd.CommandType = CommandType.StoredProcedure;
				con.Open();
				cmd.Parameters.AddWithValue("@Category", ProductCategory);
				cmd.Parameters.AddWithValue("@Task", "SelectCategorylist");
				SqlDataAdapter sda = new SqlDataAdapter(cmd);
				sda.Fill(dt1);
				con.Close();
				return dt1;

			}
			catch
			{
				return null;
			}
		}
		public async Task<List<ProductMaster>> ProductReview(string companyid)
		{
			try
			{
				DataTable dt1 = new DataTable();
				SqlConnection con = new SqlConnection(Cs);
				SqlCommand cmd = new SqlCommand("Sp_Product", con);
				cmd.CommandType = CommandType.StoredProcedure;
				con.Open();
				cmd.Parameters.AddWithValue("@CompanyId", companyid);
				cmd.Parameters.AddWithValue("@Task", "Product_review");
				SqlDataAdapter sda = new SqlDataAdapter(cmd);
				sda.Fill(dt1);
				con.Close();
				List<ProductMaster> list = new List<ProductMaster>();
				foreach (DataRow dr in dt1.Rows)
				{
					ProductMaster product = new ProductMaster
					{
						ProductIcon = dr["ProductIcon"].ToString(),
						ProductId = dr["ProductId"].ToString(),
						ProductName = dr["ProductName"].ToString(),
						Admin_Approval = dr["Admin_Approval"].ToString(),
						Request_status = dr["Request_status"].ToString()
					};
					list.Add(product);
				}
				return list;
			}
			catch
			{
				return null;
			}
		}
		public async Task<bool> remove(string productid)
		{
			try
			{
				;
				SqlConnection con = new SqlConnection(Cs);
				SqlCommand cmd = new SqlCommand("Sp_Product", con);
				cmd.CommandType = CommandType.StoredProcedure;
				con.Open();
				cmd.Parameters.AddWithValue("@ProductId", productid);
				cmd.Parameters.AddWithValue("@Task", "Product_remove");
				int a = cmd.ExecuteNonQuery();
				con.Close();
				if (a > 0)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch
			{
				return false;
			}
		}
		public async Task<ProductMaster> updateproduct(string ProductId)
		{
			try
			{
				DataSet dt1 = new DataSet();
				SqlConnection con = new SqlConnection(Cs);
				SqlCommand cmd = new SqlCommand("Sp_Product", con);
				cmd.CommandType = CommandType.StoredProcedure;
				con.Open();
				cmd.Parameters.AddWithValue("@ProductId", ProductId);
				cmd.Parameters.AddWithValue("@Task", "UpdateProduct");
				SqlDataAdapter sda = new SqlDataAdapter(cmd);
				sda.Fill(dt1);
				con.Close();
				ProductMaster productMaster = new ProductMaster
				{
					ProductId = dt1.Tables[0].Rows[0]["ProductId"].ToString(),
					ProductName = dt1.Tables[0].Rows[0]["ProductName"].ToString(),
					ProductIcon = dt1.Tables[0].Rows[0]["ProductIcon"].ToString(),
					AboutProduct = dt1.Tables[0].Rows[0]["AboutProduct"].ToString(),
					Description = dt1.Tables[0].Rows[0]["Description"].ToString(),
					Website = dt1.Tables[0].Rows[0]["Website"].ToString(),
					Category = dt1.Tables[0].Rows[0]["Category"].ToString(),
					CompanyId = dt1.Tables[0].Rows[0]["CompanyId"].ToString(),
					CompanyName = dt1.Tables[0].Rows[0]["CompanyName"].ToString(),
					CompanyEmail = dt1.Tables[0].Rows[0]["CompanyEmail"].ToString(),
					CompanyWebsite = dt1.Tables[0].Rows[0]["CompanyWebsite"].ToString(),
					Twitter = dt1.Tables[0].Rows[0]["Twitter"].ToString(),
					LinkedIn = dt1.Tables[0].Rows[0]["LinkedIn"].ToString(),
					Phone = dt1.Tables[0].Rows[0]["Phone"].ToString(),
					City = dt1.Tables[0].Rows[0]["City"].ToString(),
					Zip = dt1.Tables[0].Rows[0]["Zip"].ToString(),
					Country = dt1.Tables[0].Rows[0]["Country"].ToString(),
					State = dt1.Tables[0].Rows[0]["State"].ToString(),
					CategoryValue = dt1.Tables[0].Rows[0]["Category"].ToString(),
					LanguageValue = dt1.Tables[0].Rows[0]["SupportLanguages"].ToString(),
					HQLocation = dt1.Tables[0].Rows[0]["HQLocation"].ToString(),
				};
				foreach (DataRow dataRow in dt1.Tables[4].Rows)
				{
					productMaster.Product_imagesFile.Add(new ProductImage
					{
						ImageId = (int)dataRow["ImageId"],
						ImagesPath = dataRow["Images"].ToString(),
						Product_Id = dataRow["ProductId"].ToString()
					});
				}
				foreach (DataRow dataRow in dt1.Tables[3].Rows)
				{
					productMaster.Product_videosFile.Add(new Video
					{
						Product_Id = dataRow["ProductId"].ToString(),
						VdoFileId = (int)dataRow["VdoFileId"],
						Path = dataRow["FilePath"].ToString()
					});
				}
				productMaster.Lang = new List<string>();
				string lang1 = dt1.Tables[0].Rows[0]["SupportLanguages"].ToString();
				string[] lang = lang1.Split(',');
				productMaster.Lang.AddRange(lang);

				foreach (DataRow dataRow in dt1.Tables[1].Rows)
				{
					productMaster.GetProduct_Comments.Add(new Product_Comment
					{
						Product_ID = dataRow["Product_ID"].ToString(),
						Admin_ID = dataRow["Admin_ID"].ToString(),
						Seller_ID = dataRow["Seller_ID"].ToString(),
						Comment = dataRow["Comment"].ToString(),
						Type = dataRow["Type"].ToString(),
						Comment_Time = Convert.ToDateTime(dataRow["Comment_Time"])
					});

				}

				if (dt1.Tables[2].Rows.Count > 0)
				{
					foreach (DataRow dr in dt1.Tables[2].Rows)
					{
						productMaster.Category_list.Add(dr["category_name"].ToString()

						);
					}
				}
				return productMaster;
			}

			catch
			{
				return null;
			}



		}
		public DataTable updatedetails(ProductMaster model)
		{
			try
			{

				DataTable dt = new DataTable();
				SqlConnection con = new SqlConnection(Cs);
				SqlCommand cmd = new SqlCommand("Sp_Product", con);
				cmd.CommandType = CommandType.StoredProcedure;
				con.Open();
				cmd.Parameters.AddWithValue("@CompanyId", model.CompanyId);
				cmd.Parameters.AddWithValue("@CompanyName", model.CompanyName);
				cmd.Parameters.AddWithValue("@CompanyEmail", model.Company_Email);
				cmd.Parameters.AddWithValue("@Phone", model.Phone);
				cmd.Parameters.AddWithValue("@CompanyWebsite", model.CompanyWebsite);
				cmd.Parameters.AddWithValue("@Twitter", model.Twitter);
				cmd.Parameters.AddWithValue("@LinkedIn", model.LinkedIn);
				cmd.Parameters.AddWithValue("@HQLocation", model.HQLocation);
				cmd.Parameters.AddWithValue("@Comment", model.Notes);
				cmd.Parameters.AddWithValue("@ProductId", model.id);
				cmd.Parameters.AddWithValue("@ProductIcon", model.ProductIcon);
				cmd.Parameters.AddWithValue("@Images", model.Images);
				cmd.Parameters.AddWithValue("@ProductName", model.ProductName);
				cmd.Parameters.AddWithValue("@Description", model.Description);
				cmd.Parameters.AddWithValue("@Category", model.Category);
				cmd.Parameters.AddWithValue("@AboutProduct", model.AboutProduct);


				cmd.Parameters.AddWithValue("@Website", model.Website);
				cmd.Parameters.AddWithValue("@SupportLanguages", model.SupportLanguages);
				//cmd.Parameters.AddWithValue("@PdfFileList", model.PdfFileList);
				cmd.Parameters.AddWithValue("@ImageList", model.ImagesList);
				cmd.Parameters.AddWithValue("@ProductVideo", model.VideoList);
				cmd.Parameters.AddWithValue("@Task", "update");

				SqlDataAdapter sda = new SqlDataAdapter(cmd);
				sda.Fill(dt);
				con.Close();
				return dt;



			}





			catch
			{
				return null;
			}

		}



		public async Task<ProductMaster> selectfeatures(string ProductId)
		{
			try
			{
				DataSet dt1 = new DataSet();
				SqlConnection con = new SqlConnection(Cs);
				SqlCommand cmd = new SqlCommand("Sp_Product", con);
				cmd.CommandType = CommandType.StoredProcedure;
				con.Open();
				cmd.Parameters.AddWithValue("@ProductId", ProductId);

				cmd.Parameters.AddWithValue("@Task", "select_features");

				SqlDataAdapter sda = new SqlDataAdapter(cmd);
				sda.Fill(dt1);
				con.Close();
				ProductMaster productMaster = new ProductMaster();
				productMaster.ProductId = ProductId;
				//productMaster.Features1 = new List<string>();
				//string FeaturesGroup1 = dt1.Tables[0].Rows[0]["FeaturesGroup1"].ToString();
				//string[] frtgrps1 = FeaturesGroup1.Split('&');
				//productMaster.Features1.AddRange(frtgrps1);

				//productMaster.Features2 = new List<string>();
				//string FeaturesGroup2 = dt1.Tables[0].Rows[0]["FeaturesGroup2"].ToString();
				//string[] frtgrps2 = FeaturesGroup2.Split('&');
				//productMaster.Features2.AddRange(frtgrps2);

				//productMaster.Features3 = new List<string>();
				//string FeaturesGroup3 = dt1.Tables[0].Rows[0]["FeaturesGroup3"].ToString();
				//string[] frtgrps3 = FeaturesGroup3.Split('&');
				//productMaster.Features3.AddRange(frtgrps3);

				if (dt1.Tables[0].Rows[0]["FeaturesGroup1"] != null && !string.IsNullOrWhiteSpace(dt1.Tables[0].Rows[0]["FeaturesGroup1"].ToString()))
				{
					//set features table to list
					productMaster.Features1 = new List<string>();
					string FeaturesGroup1 = dt1.Tables[0].Rows[0]["FeaturesGroup1"].ToString();
					string[] frtgrps1 = FeaturesGroup1.Split('&');
					productMaster.Features1.AddRange(frtgrps1);
				}
				if (dt1.Tables[0].Rows[0]["FeaturesGroup2"] != null && !string.IsNullOrWhiteSpace(dt1.Tables[0].Rows[0]["FeaturesGroup2"].ToString()))
				{
					productMaster.Features2 = new List<string>();
					string FeaturesGroup2 = dt1.Tables[0].Rows[0]["FeaturesGroup2"].ToString();
					string[] frtgrps2 = FeaturesGroup2.Split('&');
					productMaster.Features2.AddRange(frtgrps2);
				}
				if (dt1.Tables[0].Rows[0]["FeaturesGroup3"] != null && !string.IsNullOrWhiteSpace(dt1.Tables[0].Rows[0]["FeaturesGroup3"].ToString()))
				{
					productMaster.Features3 = new List<string>();
					string FeaturesGroup3 = dt1.Tables[0].Rows[0]["FeaturesGroup3"].ToString();
					string[] frtgrps3 = FeaturesGroup3.Split('&');
					productMaster.Features3.AddRange(frtgrps3);
				}
				if (dt1.Tables[0].Rows[0]["Add_featuresg1"] != null && !string.IsNullOrWhiteSpace(dt1.Tables[0].Rows[0]["Add_featuresg1"].ToString()))
				{
					productMaster.featuresg1_Added = new List<string>();
					string Add_features1 = dt1.Tables[0].Rows[0]["Add_featuresg1"].ToString();
					string[] add1 = Add_features1.Split('&');
					productMaster.featuresg1_Added.AddRange(add1);
				}
				if (dt1.Tables[0].Rows[0]["Add_featuresg2"] != null && !string.IsNullOrWhiteSpace(dt1.Tables[0].Rows[0]["Add_featuresg2"].ToString()))
				{
					productMaster.featuresg2_Added = new List<string>();
					string Add_features2 = dt1.Tables[0].Rows[0]["Add_featuresg2"].ToString();
					string[] add2 = Add_features2.Split('&');
					productMaster.featuresg2_Added.AddRange(add2);

				}
				if (dt1.Tables[0].Rows[0]["Add_featuresg3"] != null && !string.IsNullOrWhiteSpace(dt1.Tables[0].Rows[0]["Add_featuresg3"].ToString()))
				{
					productMaster.featuresg3_Added = new List<string>();
					string Add_features3 = dt1.Tables[0].Rows[0]["Add_featuresg3"].ToString();
					string[] add3 = Add_features3.Split('&');
					productMaster.featuresg3_Added.AddRange(add3);
				}




				if (dt1.Tables[1].Rows.Count > 0)
				{
					foreach (DataRow dataRow in dt1.Tables[1].Rows)
					{
						productMaster.GetFeatures_Comments.Add(new Features_Comment
						{
							Product_ID = dataRow["Product_ID"].ToString(),
							Admin_ID = dataRow["Admin_ID"].ToString(),
							Seller_ID = dataRow["Seller_ID"].ToString(),
							Comment = dataRow["Comment"].ToString(),
							Type = dataRow["Type"].ToString(),
							Comment_Time = Convert.ToDateTime(dataRow["Comment_Time"])
						});
					}
				}

				return productMaster;
			}
			catch
			{
				return null;
			}

		}

		public async Task<bool> UpdateFeatures(features model)
		{
			try
			{


				SqlConnection con = new SqlConnection(Cs);
				SqlCommand cmd = new SqlCommand("Sp_Product", con);
				cmd.CommandType = CommandType.StoredProcedure;
				con.Open();
				cmd.Parameters.AddWithValue("@ProductId", model.id);
				cmd.Parameters.AddWithValue("@CompanyId", model.CompanyId);
				cmd.Parameters.AddWithValue("@FeaturesGroup3", model.FeaturesGroup3);
				cmd.Parameters.AddWithValue("@FeaturesGroup2", model.FeaturesGroup2);
				cmd.Parameters.AddWithValue("@FeaturesGroup1", model.FeaturesGroup1);
				cmd.Parameters.AddWithValue("@Add_featuresg1", model.Add_featuresg1);
				cmd.Parameters.AddWithValue("@Add_featuresg2", model.Add_featuresg2);
				cmd.Parameters.AddWithValue("@Add_featuresg3", model.Add_featuresg3);
				cmd.Parameters.AddWithValue("@Comment", model.Features_Commentgroup);

				cmd.Parameters.AddWithValue("@Task", "UpdateFeatures");
				int a = await cmd.ExecuteNonQueryAsync();

				con.Close();
				if (a > 0)
				{
					return true;
				}
				else
				{
					return false;
				}

			}

			catch
			{
				return false;
			}

		}

		
		public async Task<ProductMaster> selectPriceList(string ProductId)
		{
			try
			{
				DataSet dt1 = new DataSet();
				SqlConnection con = new SqlConnection(Cs);
				SqlCommand cmd = new SqlCommand("Sp_Product", con);
				cmd.CommandType = CommandType.StoredProcedure;
				con.Open();
				cmd.Parameters.AddWithValue("@ProductId", ProductId);

				cmd.Parameters.AddWithValue("@Task", "SelectPrice");

				SqlDataAdapter sda = new SqlDataAdapter(cmd);
				sda.Fill(dt1);
				con.Close();
				ProductMaster productMaster = new ProductMaster
				{
					//BusinessPrice = dt1.Tables[0].Rows[0]["BusinessPrice"].ToString(),
					//IndividualPrice = dt1.Tables[0].Rows[0]["IndividualPrice"].ToString(),
					//EnterprisePrice = dt1.Tables[0].Rows[0]["EnterprisePrice"].ToString(),
					//BusinessPremiumPrice = dt1.Tables[0].Rows[0]["BusinessPremiumPrice"].ToString(),
					//StandardPrice = dt1.Tables[0].Rows[0]["StandardPrice"].ToString(),
					ProductId = dt1.Tables[0].Rows[0]["ProductId"].ToString(),
					Mergin = dt1.Tables[2].Rows[0]["Margin"].ToString(),
					Product_Url = dt1.Tables[2].Rows[0]["Product_Url"].ToString()

				};

				foreach (DataRow dataRow in dt1.Tables[0].Rows)
				{
					productMaster.subscriptions.Add(new subscription
					{
						Plan_Name = dataRow["Plan_Name"].ToString(),
						Plan_tenure = dataRow["Plan_tenure"].ToString(),
						Price = (decimal)dataRow["Price"],
						Description = dataRow["Description"].ToString(),
						Plan_uniqueId = dataRow["Plan_uniqueId"].ToString()
					});



				}

				if (dt1.Tables[1].Rows.Count > 0)
				{
					foreach (DataRow dataRow in dt1.Tables[1].Rows)
					{
						productMaster.GetPrice_Comments.Add(new Price_Comment
						{
							Product_ID = dataRow["Product_ID"].ToString(),
							Admin_ID = dataRow["Admin_ID"].ToString(),
							Seller_ID = dataRow["Seller_ID"].ToString(),
							Comment = dataRow["Comment"].ToString(),
							Type = dataRow["Type"].ToString(),
							Comment_Time = Convert.ToDateTime(dataRow["Comment_Time"])
						});
					}
				}
				if (dt1.Tables[2].Rows.Count > 0)
				{
					foreach (DataRow dataRow in dt1.Tables[3].Rows)
					{
						productMaster.subscription_Tokens.Add(new subscription_token
						{
							product_id = dataRow["product_id"].ToString(),
							plan_tenure = dataRow["plan_tenure"].ToString(),
							Price = decimal.Parse(dataRow["Price"].ToString()),
							plan_uniqueid = dataRow["plan_uniqueid"].ToString(),
							plan_type = dataRow["plan_type"].ToString(),
							token = dataRow["tokens"].ToString()
						});
					}
				}



				return productMaster;
			}
			catch
			{
				return null;
			}



		}

		public async Task<bool> updateprice(ProductMaster model)
		{
			try
			{


				SqlConnection con = new SqlConnection(Cs);
				SqlCommand cmd = new SqlCommand("Sp_Product", con);
				cmd.CommandType = CommandType.StoredProcedure;
				con.Open();
				cmd.Parameters.AddWithValue("@CompanyId", model.CompanyId);
				cmd.Parameters.AddWithValue("@ProductId", model.id);
				cmd.Parameters.AddWithValue("@Mergin", model.Mergin);
				cmd.Parameters.AddWithValue("@Product_Url", model.Product_Url);
				cmd.Parameters.AddWithValue("@Packeginglist", model.Packeglist);
				//cmd.Parameters.AddWithValue("@TokenPackegList", model.Token_List);
				cmd.Parameters.AddWithValue("@Task", "UpdateProductPrice");
				int a = await cmd.ExecuteNonQueryAsync();

				con.Close();
				if (a > 0)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch
			{
				return false;
			}
		}


		public async Task<bool> productcomment(Product_Comment model)
		{
			try
			{


				SqlConnection con = new SqlConnection(Cs);
				SqlCommand cmd = new SqlCommand("Sp_ProductComment", con);
				cmd.CommandType = CommandType.StoredProcedure;
				con.Open();
				cmd.Parameters.AddWithValue("@Seller_ID", model.Seller_ID);
				cmd.Parameters.AddWithValue("@Product_ID", model.Product_ID);
				cmd.Parameters.AddWithValue("@Comment", model.Comment);
				cmd.Parameters.AddWithValue("@Type", model.Type);
				cmd.Parameters.AddWithValue("@Task", "productComment");
				int a = await cmd.ExecuteNonQueryAsync();

				con.Close();
				if (a > 0)
				{
					return true;
				}
				else
				{
					return false;
				}

			}

			catch
			{
				return false;
			}

		}


		public async Task<List<ProductMaster>> GetProduct(string CompanyId)
		{
			try
			{
				DataTable dt = new DataTable();
				SqlConnection con = new SqlConnection(Cs);
				SqlCommand cmd = new SqlCommand("Sp_Product", con);
				cmd.CommandType = CommandType.StoredProcedure;
				con.Open();
				cmd.Parameters.AddWithValue("@CompanyId", CompanyId);
				cmd.Parameters.AddWithValue("@Task", "purchase_product");
				SqlDataAdapter sda = new SqlDataAdapter(cmd);
				sda.Fill(dt);
				con.Close();

				List<ProductMaster> product = new List<ProductMaster>();
				if (dt.Rows.Count > 0)
				{
					for (int i = 0; dt.Rows.Count > i; i++)
					{

						ProductMaster productMaster = new ProductMaster();
						productMaster.ProductIcon = dt.Rows[i]["ProductIcon"].ToString();
						productMaster.ProductId = dt.Rows[i]["ProductId"].ToString();
						productMaster.ProductName = dt.Rows[i]["ProductName"].ToString();
						productMaster.Product_Url = dt.Rows[i]["Product_Url"].ToString();
						//productMaster.token = dt.Rows[i]["token"].ToString();
						productMaster.FirstName = dt.Rows[i]["First_Name"].ToString();
						productMaster.LastName = dt.Rows[i]["Last_Name"].ToString();
						productMaster.Company_Email = dt.Rows[i]["CompanyEmail"].ToString();
						productMaster.GetSubscription.Plan_uniqueId = $"{dt.Rows[i]["PlanId"]}";
						product.Add(productMaster);

					}


				}
				return product;
			}
			catch
			{
				return null;
			}
		}

		public async Task<ProductMaster> GetProductWithCompany(string CompanyId, string productId,string PlanId)
		{
			try
			{
				DataSet ds = new DataSet();
				SqlConnection con = new SqlConnection(Cs);
				SqlCommand cmd = new SqlCommand("Sp_Product", con);
				cmd.CommandType = CommandType.StoredProcedure;
				con.Open();
				cmd.Parameters.AddWithValue("@CompanyId", CompanyId);
				cmd.Parameters.AddWithValue("@ProductId", productId);
				cmd.Parameters.AddWithValue("@PlanId", PlanId);
				cmd.Parameters.AddWithValue("@Task", "ProductWithCompany");
				SqlDataAdapter sda = new SqlDataAdapter(cmd);
				sda.Fill(ds);
				con.Close();
				ProductMaster product = new ProductMaster();
				if (ds.Tables.Count > 0)
				{
					
					product.ProductId = $"{ds.Tables[0].Rows[0]["ProductId"]}";
					product.Product_Url = $"{ds.Tables[0].Rows[0]["Product_Url"]}";
					product.token = $"{ds.Tables[0].Rows[0]["Token"]}";
					product.Company_Email = $"{ds.Tables[1].Rows[0]["CompanyEmail"]}";
					product.First_Name = $"{ds.Tables[1].Rows[0]["First_Name"]}";
					product.Last_Name = $"{ds.Tables[1].Rows[0]["Last_Name"]}";
					product.GetSubscription.Plan_Name = $"{ds.Tables[2].Rows[0]["Plan_Name"]}";
					product.GetSubscription.Plan_uniqueId = $"{ds.Tables[2].Rows[0]["Plan_uniqueId"]}";
					product.result = $"{ds.Tables[3].Rows[0]["Result"]}";
				}
				return product;
			}
			catch (Exception Ex)
			{
				ProductMaster product = new ProductMaster();
				return product;
			}
		}

		public async Task<ProductMaster> GetProduct_Sales_Details(int productId)
		{
			try
			{
				DataSet ds = new DataSet();
				SqlConnection con = new SqlConnection(Cs);
				SqlCommand cmd = new SqlCommand("Sp_Product", con);
				cmd.CommandType = CommandType.StoredProcedure;
				con.Open();

				cmd.Parameters.AddWithValue("@ProductId", productId);
				cmd.Parameters.AddWithValue("@Task", "Product_salesDetails");

				SqlDataAdapter sda = new SqlDataAdapter(cmd);
				sda.Fill(ds);
				con.Close();


				ProductMaster products = new ProductMaster
				{
					ProductId = Convert.ToString(productId),
					AboutProduct = ds.Tables[0].Rows[0]["AboutProduct"].ToString(),
					ProductName = ds.Tables[0].Rows[0]["ProductName"].ToString(),
					ProductIcon = ds.Tables[0].Rows[0]["ProductIcon"].ToString(),
					Category = ds.Tables[0].Rows[0]["Category"].ToString(),
					Description = ds.Tables[0].Rows[0]["Description"].ToString(),
					Mergin = ds.Tables[0].Rows[0]["Margin"].ToString(),




				};
				products.subscriptions = new List<subscription>();
				foreach (DataRow dataRow in ds.Tables[1].Rows)
				{
					products.subscriptions.Add(new subscription
					{
						Plan_Name = dataRow["plan_type"].ToString(),

						Price = (decimal)dataRow["Price"],
						Token_Count = (int)dataRow["Token_Count"],
						Sold_Count = (int)dataRow["Sold_Count"]
					});

				}
				return products;

			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public async Task<List<ProductMaster>> ProdctList(string companyid)
		{
			try
			{
				DataTable ds1 = new DataTable();
				SqlConnection con = new SqlConnection(Cs);
				SqlCommand cmd = new SqlCommand("Sp_Product", con);
				cmd.CommandType = CommandType.StoredProcedure;
				con.Open();

				cmd.Parameters.AddWithValue("@CompanyId", companyid);
				cmd.Parameters.AddWithValue("@Task", "Product_List");
				SqlDataAdapter sda = new SqlDataAdapter(cmd);
				sda.Fill(ds1);
				con.Close();

				List<ProductMaster> list = new List<ProductMaster>();
				foreach (DataRow dr in ds1.Rows)
				{
					ProductMaster product = new ProductMaster
					{

						ProductIcon = dr["ProductIcon"].ToString(),
						ProductId = dr["ProductId"].ToString(),
						ProductName = dr["ProductName"].ToString(),

						date = (DateTime)dr["Date"],
						Category = dr["Category"].ToString()
					};
					list.Add(product);
				}


				return list;

			}
			catch
			{
				return null;
			}
		}
	}
}