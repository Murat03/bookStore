using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
	public class PriceOutOfRangeBadRequestException : BadRequestException
	{
        public PriceOutOfRangeBadRequestException(string message = "Price should be less than 1000 and greater than 10.") : base(message){ }
    }
}
