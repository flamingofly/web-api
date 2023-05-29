using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using my_first_webapi.Data;
using my_first_webapi.Models;

namespace my_first_webapi.Controllers
{
    [ApiController]
    //[Route("api/contacts")] //first way to initialize controller
    [Route("api/[controller]")] //second way
    public class ContactsController : Controller //second way is taking control from here 
    {
        private readonly ContactsAPIDbContext dbContext;

        public ContactsController(ContactsAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet] //swagger ui use this by anotation([])
        // Async and await us it to make our method asuncrounous
        public async Task<IActionResult> GetContacts()
        {
            return Ok(await dbContext.Contacts.ToListAsync());
            
        }

        [HttpGet]
        [Route("{id:guid}")] 
        public async Task<IActionResult> GetContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);

            if(contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }


        [HttpPost]
        public async Task<IActionResult> AddContact(AddContactRequest addContactRequest)
        {
            var contact = new Contact() // this goes in database
            {
                Id = Guid.NewGuid(),
                Address = addContactRequest.Address,
                Email = addContactRequest.Email,
                FullName = addContactRequest.FullName,
                Phone = addContactRequest.Phone

            };

            //dbContext.Contacts.Add(contact); //type 1
            await dbContext.Contacts.AddAsync(contact); //type 2
            await dbContext.SaveChangesAsync();

            return Ok(contact);
        }



        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, UpdateContactRequest updateContactRequest)
        {
            var contact = await dbContext.Contacts.FindAsync(id);
            if(contact != null)
            {
                contact.FullName = updateContactRequest.FullName;  
                contact.Address = updateContactRequest.Address; 
                contact.Phone = updateContactRequest.Phone;
                contact.Email = updateContactRequest.Email;

                await dbContext.SaveChangesAsync();

                return Ok(contact);

            }
            return NotFound();
        

        }


        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);

            if(contact != null)
            {
                dbContext.Remove(contact);
                await dbContext.SaveChangesAsync();
                //return Ok(contact);
                return Ok("This contact has been deleted");
            }

            return NotFound();
        }

    }

}
