using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eventor.Models.App
{
    public class ImportarContactos
    {
        public class GoogleContacts
        {
            public Feed Feed { get; set; }
        }

        public class Feed
        {
            public GoogleTitle Title { get; set; }
            public List<Contact> Entry { get; set; }
        }

        public class GoogleTitle
        {
            public string T { get; set; }
        }

        public class Contact
        {
            public GoogleTitle Title { get; set; }
            public List<Email> GdEmail { get; set; }
            public List<Link> Link { get; set; }
        }
        public class Email
        {
            public string Address { get; set; }
            public bool Primary { get; set; }
        }

        public class Link
        {
            public string Rel { get; set; }
            public string Type { get; set; }
            public string Href { get; set; }
        }
    }
}