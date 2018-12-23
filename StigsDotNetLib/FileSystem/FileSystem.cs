using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StigsDotNetLib.FileSystem
{
	/// <summary>
	/// A wrapper around common file system classes such as File and Directory
	/// Using this wrapper instead of the wrapped classes provides a test seam.
	/// </summary>
    public class FileSystem
    {
        public static FileSystem Default { get; set; }
	//	public static MakeDirectory 


    }
}
