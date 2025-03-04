Un ristorante ci chiede di realizzare un gestionale per semplificare la gestione dei suoi piatti e dei suoi men�.

Ogni men� pu� avere pi� piatti, e un piatto pu� essere inserito in pi� men�. Ogni piatto deve far parte di una categoria (primo piatto, secondo piatto, contorno...), ma una categoria pu� contenere pi� piatti.

Un men� � caratterizzato da un nome.
Un piatto � caratterizzato da un nome, una descrizione e un prezzo.
Una categoria � caratterizzata da un nome.

Il cliente finale deve poter accedere a un'applicazione con cui visualizzare e manipolare tutte le informazioni a disposizione: visualizzare i men�, le categorie, i piatti, modificarli, cancellarli o aggiungerne di nuovi.

--

Dalle richieste del cliente strutturiamo la soluzione usando diversi strumenti:

1) Creiamo un database avvalendoci delle informazioni date (che tabelle creiamo? Che relazioni abbiamo fra le entit�?)
2) Creiamo un back-end (ASP.Net web API) che faccia uso di tutte le nozioni apprese (dependency injection, repository pattern...) per esporre al mondo delle API di visualizzazione/modifica di tutte le informazioni richieste (men�, piatti...)
3) Creiamo un front-end (applicazione desktop WPF) che si interfacci con le API esposte dal back-end