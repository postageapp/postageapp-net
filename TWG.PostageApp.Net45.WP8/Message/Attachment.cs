using System;
using System.IO;

namespace TWG.PostageApp.Message
{
    /// <summary>
    /// Represents message attachment.
    /// </summary>
    public class Attachment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Attachment"/> class.
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        /// <param name="fileName">
        /// The filename.
        /// </param>
        /// <param name="contentType">
        /// The content type.
        /// </param>
        public Attachment(Stream stream, string fileName, string contentType = "application/octet-stream")
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            ContentStream = stream;
            ContentType = contentType;
            FileName = fileName;
        }

        /// <summary>
        /// Gets file name.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Gets content type.
        /// </summary>
        public string ContentType { get; private set; }

        /// <summary>
        /// Gets content.
        /// </summary>
        public Stream ContentStream { get; private set; }
    }
}