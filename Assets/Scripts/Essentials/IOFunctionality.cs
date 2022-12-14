using UnityEngine;
using System.IO;

namespace AustenKinney.Essentials
{
    public class IOFunctionality : MonoBehaviour
    {
        /// <summary>
        /// Gets the subdirectories within the directory at the given path.
        /// </summary>
        /// <param name="rootPath">The path for the directory whose contents wwill be checked.</param>
        /// <returns>DirectoryInfo[]</returns>
        public static DirectoryInfo[] GetDirectories(string rootPath)
        {
            DirectoryInfo root = new DirectoryInfo(rootPath);
            DirectoryInfo[] subdirectories = root.GetDirectories();
            return subdirectories;
        }
    }
}
