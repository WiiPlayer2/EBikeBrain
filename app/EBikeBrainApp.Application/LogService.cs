using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace EBikeBrainApp.Application;

public class LogService : IDisposable
{
    private const int MAX_LOG_ENTRY_COUNT = 50;

    private readonly BehaviorSubject<Lst<LogEntry>> logsSubject = new(List<LogEntry>());

    private readonly CompositeDisposable subscriptions;

    public LogService(IEventStream<LogEntry> logs)
    {
        subscriptions = new CompositeDisposable(
            logs
                .Scan(
                    List<LogEntry>(),
                    (acc, cur) => toList(acc.Add(cur).TakeLast(MAX_LOG_ENTRY_COUNT)))
                .Subscribe(logsSubject));
    }

    public IObservable<Lst<LogEntry>> LogEntries => logsSubject;

    public void Dispose()
    {
        subscriptions.Dispose();
        logsSubject.Dispose();
    }
}
