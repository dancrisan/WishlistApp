namespace WishlistApp.Lib

open System
open System.Collections.Generic
open System.IO
open System.Windows
open System.Windows.Media
open System.Windows.Media.Imaging


type Image private (image : BitmapSource) =
    new (sourceStream : Stream) =
        let image = new BitmapImage ()
        image.BeginInit ()
        image.StreamSource <- sourceStream
        image.EndInit ()
        Image (image)

    member x.Resize maxWidth maxHeight =
        let xscale = (maxWidth / float image.PixelWidth)
        let yscale = (maxHeight / float image.PixelHeight)
        Image (new TransformedBitmap (image, ScaleTransform (xscale, yscale)))

    member x.SerializeJpeg () =
        use stream = new MemoryStream ()
        let enc =
            new JpegBitmapEncoder (
                QualityLevel = 90)
        enc.Frames.Add (BitmapFrame.Create image)
        enc.Save stream
        let data = stream.ToArray ()
        data

