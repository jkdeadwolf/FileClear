string usage = """
    fileclear path [-t days] [-d destpath] 
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
            File.Move(file, job.DestPath);
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
            File.Delete(file);
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
    clearJob.Path = args[1];
    clearJob.TimeSpan = new TimeSpan(365, 0, 0, 0);
    clearJob.DestPath ="";
    return clearJob;
}

class ClearJob
{

    public string Path { get; set; }

    public TimeSpan TimeSpan { get; set; }

    public string DestPath { get; set; }
}