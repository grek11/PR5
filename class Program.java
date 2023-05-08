public class Program
{
    static void Main(string[] args)
    {
        Store store = new Store();
        Producer producer = new Producer(store);
        Consumer consumer = new Consumer(store);
        new Thread(producer).start();
        new Thread(consumer).start();
    }
}

class Store
{
    private int product = 0;
    public synchronized void get()
    {
        while (product < 1)
        {
            try
            {
                wait();
            }
            catch (IntrerruptedException e) { }
        }
        product--;
        System.out.println("Покупець купив 1 товар");
        System.out.println("Товарів на складі: ");
        notify();
    }
    public synchronized void put() {
        while (product >= 3) {
            try {
                wait();
            }
            catch (IntrerruptedException e) { }
        }
        product++;
        System.out.println("Виробник додав 1 товар");
        System.out.println("Товарів на складі: " + product);
        notify();
    }
}

class Producer implements Runnable {

    Store store;
    Producer(Store store) {
        this.store = store;
    }

    public void run() {
        for (int i = 1; i < 6; i++) {
            store.put();
        }
    }
}

class Consumer implements Runnable {

    Store store;
    Consumer(Store store) {
        this.store = store;
    }

    public void run() {
        for (int i = 1; i < 6; i++) {
            store.get();
        }
    }
}