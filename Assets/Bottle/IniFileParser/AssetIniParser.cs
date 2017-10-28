using System;
using System.IO;
using System.Text;
using IniParser.Exceptions;
using IniParser.Model;
using UnityEngine;

namespace IniParser
{
	/// <summary>
	/// Represents an readonly INI data parser for assets.
	/// </summary>
	public class AssetIniDataParser : StreamIniDataParser
	{
		/// <summary>
		///     Implements reading ini data from an asset.
		/// </summary>
		/// <remarks>
		///     Uses <see cref="Encoding.Default"/> codification for the asset.
		/// </remarks>
		/// <param name="assetName">
		///     Name of the asset.
		/// </param>
		public IniData ReadAsset(string assetName)
		{
			return ReadAsset(assetName, Encoding.Default);
		}

		/// <summary>
		///     Implements reading ini data from an Asset.
		/// </summary>
		/// <param name="assetName">
		///     Name of the asset.
		/// </param>
		/// <param name="fileEncoding">
		///     File's encoding.
		/// </param>
		public IniData ReadAsset(string assetName, Encoding fileEncoding)
		{
			if (string.IsNullOrEmpty(assetName))
				throw new ArgumentException("Bad asset name.");

			try
			{
				var textAsset = Resources.Load(assetName, typeof (TextAsset));
				using (var sr = new StreamReader(new MemoryStream((textAsset as TextAsset).bytes)))
				{
					return ReadData(sr);
				}
			}
			catch (IOException ex)
			{
				throw new ParsingException(String.Format("Could not parse asset {0}", assetName), ex);
			}

		}

	}
}
