open System.IO
open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats
open CommandLine
open Spectre.Console

type Options = {
    [<Option('d', "dir", HelpText = "Directory containing GIFs to process.", Default="./")>] directory : string;
    [<Option('r', "recursive", HelpText = "Recursively scan directories from root directory", Default=false)>] recursive: bool
}

let getFrameFromGif (path : string) =
    let gif = Image.Load(path)
    let frame = gif.Frames.RootFrame
    use image = new Image<Rgba32>(frame.Width, frame.Height)
    let source = frame :?> ImageFrame<Rgba32>
    for y in 0..frame.Height-1 do
        for x in 0..frame.Width-1 do
            image.[x,y] <- source.[x,y]
    use ms = new MemoryStream()
    image.SaveAsPng(ms)
    ms.ToArray()

let processFiles options =
    match Directory.Exists options.directory with
    | true ->
        let searchOption = if options.recursive then SearchOption.AllDirectories else SearchOption.TopDirectoryOnly
        let files = Directory.GetFiles(options.directory, "*.gif", searchOption)
        let progress = AnsiConsole.Status()
        let processEachFile _ =
            for path in files do
                let bytes = getFrameFromGif path
                let filename = Path.GetFileNameWithoutExtension path
                let target = $"{filename}.png"
                AnsiConsole.MarkupLine $"[green]:check_mark: Processing {Path.GetFileName path} -> {target}[/]"
                File.WriteAllBytes(target, bytes)
        progress.Start($"processing {files.Length} GIFs", processEachFile)
        AnsiConsole.MarkupLine($"[green]:glowing_star: { files.Length} GIFs processed in {options.directory}[/]")
    | false ->
        AnsiConsole.MarkupLine $"[red]:exclamation_question_mark: Directory '{options.directory}' does not exist[/]"

[<EntryPoint>]
let main argv =
    let result = Parser.Default.ParseArguments<Options>(argv)
    match result with
    | :? Parsed<Options> as parsed -> processFiles parsed.Value
    | :? NotParsed<Options> as incorrect -> printf $"Invalid: %A{argv}, Errors: %u{Seq.length incorrect.Errors}"
    | _ -> failwith "Unknown parsing error"
    0