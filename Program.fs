open System.IO
open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats
open CommandLine
open Spectre.Console
open type Emoji.Known

type options = {
    [<Option('d', "dir", HelpText = "Directory containing GIFs to process.", Default="./")>] directory : string;
    [<Option('r', "recursive", HelpText = "Recursively scan directories from root directory", Default=false)>] recursive: bool
}

let getFrameFromGif (path: string) : byte[] option =
    let gif = Image.Load(path)
    let frame = gif.Frames.RootFrame
    use image = new Image<Rgba32>(frame.Width, frame.Height)
    match frame with
    | :? ImageFrame<Rgba32> as source ->
        for y in 0..frame.Height-1 do
            for x in 0..frame.Width-1 do
                image.[x,y] <- source.[x,y]
        use ms = new MemoryStream()
        image.SaveAsPng(ms)
        Some (ms.ToArray())
    | _ ->
        None

let processFiles options : Unit =
    match Directory.Exists options.directory with
    | true ->
        let searchOption = if options.recursive then SearchOption.AllDirectories else SearchOption.TopDirectoryOnly
        let files = Directory.GetFiles(options.directory, "*.gif", searchOption)
        let progress = AnsiConsole.Status()
        let processEachFile (ctx:StatusContext) =
            for path in files do
                let bytes = getFrameFromGif path
                match bytes with
                | Some bytes ->
                    let filename = Path.GetFileNameWithoutExtension path
                    let target = $"%s{filename}.png"
                    AnsiConsole.MarkupLine $"[green]{CheckMark} Processing %s{Path.GetFileName path} -> %s{target}[/]"
                    File.WriteAllBytes(target, bytes)
                | None ->
                    AnsiConsole.MarkupLine $"[yellow]{Warning} Image {Path.GetFileName path} was not a valid ImageFrame, skipping[/]"

        progress.Start($"processing {files.Length} GIFs", processEachFile)
        AnsiConsole.MarkupLine($"[green]{GlowingStar} { files.Length} GIFs processed in {options.directory}[/]")
    | false ->
        AnsiConsole.MarkupLine $"[red]{ExclamationQuestionMark} Directory '{options.directory}' does not exist[/]"

[<EntryPoint>]
let main argv =
    let result = Parser.Default.ParseArguments<options>(argv)
    match result with
    | :? Parsed<options> as parsed -> processFiles parsed.Value
    | :? NotParsed<options> as incorrect -> printf $"Invalid: %A{argv}, Errors: %u{Seq.length incorrect.Errors}"
    | _ -> failwith "Unknown parsing error"
    0