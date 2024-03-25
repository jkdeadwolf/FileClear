string usage = """
    fileclear path [destpath] [-t days] 
    """;

ClearJob? job = GetJob();
if (job == null) return;
var files = Directory.GetFiles(job.Path, "", SearchOption.AllDirectories);
DateTime now = DateTime.Now;
if (string.IsNullOrEmpty(job.DestPath) == false)
{
    foreach (var file in files)
    {
        FileInfo fileInfo = new FileInfo(file);
        if (fileInfo.LastWriteTime.Add(job.TimeSpan) < now)
        {
            Console.WriteLine($"MOVE {file}");
            try
            {
                File.Move(file, job.DestPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MOVE {file} Exception:{ex.Message}");
            }
        }
    }
}
else
{
    foreach (var file in files)
    {
        FileInfo fileInfo = new FileInfo(file);
        if (fileInfo.LastWriteTime.Add(job.TimeSpan) < now)
        {
            Console.WriteLine($"DELETE {file}");
            try
            {
                File.Delete(file);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DELETE {file} Exception:{ex.Message}");
            }
        }
    }
}


ClearJob? GetJob()
{
    if (args.Length == 0)
    {
        Console.WriteLine(usage);
        return null;
    }
    ClearJob clearJob = new ClearJob();
    
    clearJob.Path = args[0];
    if (args.Length == 2)
        clearJob.DestPath = args[1];
    else if (args.Length == 3)
    {
        if (args[1] == "-t")
            clearJob.TimeSpan = new TimeSpan(int.Parse(args[2]), 0, 0, 0);
    }
    else if (args.Length == 4)
    {
        clearJob.DestPath = args[1];
        clearJob.TimeSpan = new TimeSpan(int.Parse(args[3]), 0, 0, 0);
    }
    else if (args.Length != 1)
    {
        Console.WriteLine(usage);
        Console.WriteLine(string.Join(" ", args));
        return null;
    }

    if (clearJob.TimeSpan == TimeSpan.Zero)
        clearJob.TimeSpan = new TimeSpan(365, 0, 0, 0);
    clearJob.DestPath ="";
    return clearJob;
}

class ClearJob
{

    public string Path { get; set; }

    public TimeSpan TimeSpan { get; set; } = TimeSpan.Zero;

    public string DestPath { get; set; }
}