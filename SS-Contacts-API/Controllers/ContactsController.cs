using LiteDB;
using SS_Contacts_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;

namespace SS_Contacts_API.Controllers
{
    public class ContactsController : ApiController
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["LiteDB"].ConnectionString;

        // GET: contacts
        public IHttpActionResult GetAllContacts()
        {
            using (var db = new LiteDatabase(connectionString))
            {
                // Get a collection (or create, if doesn't exist)
                var col = db.GetCollection<Contact>("contacts");

                // Query the collection
                var results = col.FindAll().ToList();

                // Return results of the query
                return Ok(results);
            }

        }

        // GET contacts/5
        public IHttpActionResult GetContactById(int id)
        {
            using (var db = new LiteDatabase(connectionString))
            {
                var col = db.GetCollection<Contact>("contacts");

                var result = col.FindById(id);

                if (result is null) // Check if document exists
                {
                    return NotFound();
                }

                return Ok(result);
            }

        }

        // POST contacts
        public IHttpActionResult Post([FromBody] Contact contact)
        {
            //Valdiate user input
            bool validated = ValidateInput(contact);
            if (!validated)
            {
                return BadRequest();
            }

            using (var db = new LiteDatabase(connectionString))
            {
                var col = db.GetCollection<Contact>("contacts");

                // Add new contact document to collection
                col.Insert(contact);

                // Return the added contact
                return Ok(contact);
            }

        }

        // PUT contacts/5
        public IHttpActionResult Put(int id, [FromBody] Contact contact)
        {
            // Validate user input
            bool validated = ValidateInput(contact);
            if (!validated)
            {
                return BadRequest();
            }

            using (var db = new LiteDatabase(connectionString))
            {

                var col = db.GetCollection<Contact>("contacts");


                var orignalDocument = col.FindById(id); // Check if original document exists
                if (orignalDocument is null)
                {
                    return NotFound();
                }

                contact.Id = id; // Insert id of contact into object

                bool updated = col.Update(contact);

                if (!updated) // Check if update failed
                {
                    return InternalServerError();
                }

                // Return the updated contact
                return Ok(contact);
            }
        }

        // DELETE contacts/5
        public IHttpActionResult Delete(int id)
        {

            using (var db = new LiteDatabase(connectionString))
            {
                var col = db.GetCollection<Contact>("contacts");

                var originalDocument = col.FindById(id); // Check if original doc exists
                if (originalDocument is null)
                {
                    return NotFound();
                }

                // Delete contact from collection
                bool deleted = col.Delete(id);

                if (!deleted) // Check if delete failed
                {
                    return InternalServerError();
                }

                return Ok($"Contact {id} deleted.");
            }
        }

        private bool ValidateInput(Contact contact)
        {
            // Validate phone types
            string[] validPhoneTypes = { "home", "work", "mobile" };

            foreach (Phone phone in contact.Phone)
            {
                if (!validPhoneTypes.Contains(phone.Type))
                {
                    return false;
                }
            }

            // Validate email by using MailAddress constructor 
            // Note: This method is not 100% accurate but catches ~most~ cases w/o using regex
            try
            {
                MailAddress m = new MailAddress(contact.Email);
            }
            catch (FormatException)
            {
                return false;
            }

            return true;
        }
    }
}
