using System;
using System.Collections.Generic;
using System.Text;

namespace ecomm_project.DataAccess.repository.Irepository
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number , string message);
    }
}
