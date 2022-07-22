using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.Runtime.Caching;
using System.IdentityModel.Tokens.Jwt;

namespace WinFromAuthClient
{
    public partial class Form1 : Form
    {
        MemoryCache cache;
        object cachedObject;
        IHttpClientFactory _factory;

        public Form1(IHttpClientFactory factory)
        {
            _factory = factory;
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            cache = new MemoryCache("StackOverflow");
        }

        public async void BtnClick(object sender, EventArgs e)
        {         
            var input = JsonSerializer.Serialize(new { username = textBox1.Text, password = textBox2.Text });

            var reqContent = new StringContent(input, Encoding.UTF8, "application/json");

            var hc = _factory.CreateClient("MyClient");

            var result = await hc.PostAsync("https://172.16.53.174:883/api/v1.0/login/verify", reqContent);

            label1.Text = !result.IsSuccessStatusCode ? "Disconnected" : "Connected";

            var content = await result.Content.ReadAsStringAsync();

            var token = JsonSerializer.Deserialize<Token>(content);

            var handler = new JwtSecurityToken(token.token);

            var id = handler.Claims.First(c => c.Type == "id").Value;
            var username = handler.Claims.First(c => c.Type == "username").Value;
            var name = handler.Claims.First(c => c.Type == "name").Value;
            var supplierId = handler.Claims.First(c => c.Type == "supplierid").Value;
            var supplierName = handler.Claims.First(c => c.Type == "suppliername").Value;

            if (token.token == null) return;

            cachedObject = cache.Get("MyCacheItem");

            if (cachedObject == null)
            {
                cache.Add("MyCacheItem", token.token, DateTime.Now.AddDays(1));
            }

            label2.Text = cache.Get("MyCacheItem").ToString();
        }

        public async void btn2Click(object sender, EventArgs e)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:5001/api/v1.0/login/hello");
                var tokens = "Bearer " + cache.Get("MyCacheItem").ToString();
                request.Headers.Add("Authorization", tokens);
                var hc = _factory.CreateClient();
                var response = await hc.SendAsync(request);

                if(response.IsSuccessStatusCode)
                {
                    label3.Text = "Successfully Verified JWT TOKEN";
                    label3.ForeColor = Color.Green;
                }
            }
            catch(Exception ex)
            {
                label3.Text = ex.Message;
                label3.ForeColor = Color.Red;
            }
            
        }

    }
}
