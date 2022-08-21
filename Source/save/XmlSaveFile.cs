using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Utils.save {
	public class XmlSaveFile {

		public XmlWriterSettings Settings;

		private List<IXmlSaveable> WillSave = new List<IXmlSaveable>();

		public XmlSaveFile() {
			Settings = new XmlWriterSettings();
			Settings.Indent = true;
		}

		public void Add(IXmlSaveable s) {
			WillSave.Add(s);
		}

		public void Save(string path) {
			using (var writer = XmlWriter.Create(path, Settings)) {
				writer.WriteStartDocument();

				writer.WriteStartElement("level");
				writer.WriteAttributeString("format", "1");

				foreach (var e in WillSave) {
					e.Save(writer);
				}
				writer.WriteEndElement();
				writer.WriteEndDocument();
			}
			WillSave.Clear();
		}

		public DialogResult SaveAsDialog(string title, string defualtname, out SaveFileDialog output) {
			var saveto = new SaveFileDialog() {
				InitialDirectory = ContentLoader.LocalPath,
				Title = title,
				AddExtension = true,
				DefaultExt = ".xml",
				FileName = defualtname,
				Filter = "Xml Files|*.xml"
			};

			output = saveto;
			return output.ShowDialog();
		}

		public static DialogResult LoadDialog(string title, out OpenFileDialog output) {
			var open = new OpenFileDialog() {
				InitialDirectory = ContentLoader.LocalPath,
				Title = title,
				Filter = "Xml Files|*.xml"
			};

			output = open;
			return output.ShowDialog();
		}

	}
}
