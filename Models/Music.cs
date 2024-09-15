using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sonata.Models;
public class Music
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string FilePath { get; set; }
}

