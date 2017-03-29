using System;
using System.IO;
using System.Reactive.Linq;
using System.Xml;

namespace CB.CSharp.Extentions
{
    public static class StreamToXmlReader
    {
        private static IObservable<XmlReader> RxStreamObserver<T>(this Stream Stream, string fragmentElementName) =>
          Observable.Generate(
              XmlReader.Create(Stream),
              reader => !reader.EOF && reader.ReadToFollowing(fragmentElementName),
              reader => reader,
              reader => reader.ReadSubtree()
          );
    }
}