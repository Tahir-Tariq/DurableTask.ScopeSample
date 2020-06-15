# Durable scope table

sample output with 25 requests:

Processing...completed:25
Memory used before collection:       13,436,336
Memory used after full collection:   6,616,112

|Type |Created| Disposed| Finalized|
|--:|--|--|--|
|SimpleService |150| 150| 150|
|ScopedActivity |75| 75| 75|
|TransitiveActivity |75| 75| 75|
|TypedActivity |75| 0| 75|
|ActivityScope |150| 0| 150|
|ScopedOrchestration |100| 25| 25|
|TransitiveOrchestration |100| 25| 25|
|TypedOrchestration |100| 0| 25|
|MainOrchestration |100| 0| 25|
|ScopedOrchestration |100| 25| 25|