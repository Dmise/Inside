using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inside.Data

{
    public partial class Message
    {
        [Key]
        public int MessageId { get; set; }
        public string Username { get; set; }
        public string TextOfMessage { get; set; }

        //Navigation properties
        public virtual User User { get; set; }
    }

    public partial class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        //Navigation Properties
        public virtual ICollection<Message>? Messages { get; set; }
    }


}
