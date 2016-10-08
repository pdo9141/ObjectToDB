using System;

namespace ObjectToDB.Contracts
{
    [Serializable]
    public class UserDetail
    {
        public string UserName { get; set; }

        public string MailID { get; set; }

        public Guid ID { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
