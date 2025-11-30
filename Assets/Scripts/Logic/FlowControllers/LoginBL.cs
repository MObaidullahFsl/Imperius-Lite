using System;
using System.Collections.Generic;
using UnityEngine;
using Imperius.Data;
using System.Threading.Tasks;

namespace Imperius.Logic
{
    public class LoginBL
    {
        LoginDL loginDL;

        public LoginBL()
        {
            loginDL = new();
        }

        public async Task<User> loginUser(UserDTO u)
        {
            var user = await loginDL.verifyUserFromDB(u);
            Debug.Log("BL User: " + user);
            if (user == null)
            {
                Debug.Log("Login failed !");
                return null;
            }
            return user;
        }
    }
}

