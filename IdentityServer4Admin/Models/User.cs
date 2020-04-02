using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4Admin.Models
{
    public class User
    {
       
        [Key]
        public int UserID { get; set; }
        public string UserName { get; set; }

        public string UserPassWord { get; set; }
    }
}
