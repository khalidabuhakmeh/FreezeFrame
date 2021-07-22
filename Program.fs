open System.IO
open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats
open CommandLine
open Spectre.Console
open type Emoji.Known

type Options = {
    [<Option('d', "dir", HelpText = "Directory containing GIFs to process.", Default="./")>] directory : string;
    [<Option('r', "recursive", HelpText = "Recursively scan directories from root directory", Default=false)>] recursive: bool
}

let getFrameFromGif (path: string) =
    use gif = Image.Load(path)
    use image = new Image<Rgba32>(gif.Width, gif.Height)
    image.Frames.AddFrame(gif.Frames.RootFrame)|> ignore
    image.Frames.RemoveFrame(0)
    let ms = new MemoryStream()
    image.SaveAsPng(ms)
    ms.Seek(0L, SeekOrigin.Begin) |> ignore
    ms

let processFiles options : Unit =
    if Directory.Exists options.directory then
        let searchOption = if options.recursive then SearchOption.AllDirectories else SearchOption.TopDirectoryOnly
        let files = Directory.GetFiles(options.directory, "*.gif", searchOption)
        let progress = AnsiConsole.Status()
        let processEachFile (ctx:StatusContext) =
            for path in files do
                let filename = Path.GetFileNameWithoutExtension path
                let target = $"%s{filename}.png"
                use destinationStream = File.Create target
                use frameStream = getFrameFromGif path
                AnsiConsole.MarkupLine $"[green]:check_mark: Processing %s{Path.GetFileName path} -> %s{target}[/]"
                frameStream.CopyTo(destinationStream)
        progress.Start($"processing {files.Length} GIFs", processEachFile)
        AnsiConsole.MarkupLine($"[green]{GlowingStar} { files.Length} GIFs processed in {options.directory}[/]")
    else
        AnsiConsole.MarkupLine $"[red]{ExclamationQuestionMark} Directory '{options.directory}' does not exist[/]"

[<EntryPoint>]
let main argv =
    let result = Parser.Default.ParseArguments<Options>(argv)
    match result with
    | :? Parsed<Options> as parsed -> processFiles parsed.Value
    | :? NotParsed<Options> as incorrect -> printf $"Invalid: %A{argv}, Errors: %u{Seq.length incorrect.Errors}"
    | _ -> failwith "Unknown parsing error"
    0