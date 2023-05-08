import java.util.concurrent.Semaphore;

public class Ex2 {
    public static void main(String[] args) {

        Semaphore sem = new Semaphore(2);
        for (int i = 1; i < 6; i++)
        new Philosopher(sem,i).start();
    }
}

class Philosopher extends Thread
{
    Semaphore sem;
    int num = 0;
    int id;
    Philosopher(Semaphore sem, int id)
    {
        this.sem=sem;
        this.id=id;
    }

    public void run()
    {
        try 
        {
            while(num < 3)
            {
                sem.acquire();
                System.out.println("Філософ" + id + " сідає за стіл");
                sleep(500);
                num++;

                System.out.println("Філософ" + id + " виходить з-заа столу");
                sem.release();

                sleep(500);
            }
        }
        catch(InterruptedException e)
        {
            System.out.println("У філософа " + id + " проблеми із здоров'ям");
        }
    }
}
