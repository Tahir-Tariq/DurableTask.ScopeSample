# Durable scope table

sample output with 25 requests:

Processing...completed:25

Memory used before collection:       11,705,688

Memory used after full collection:   5,708,416


|Type |Created| Disposed| Finalized|
|--:|--|--|--|
|SimpleService |150| 150| 150|
|ScopedActivity |75| 75| 75|
|TransitiveActivity |75| 75| 75|
|TypedActivity |75| 0| 75|
|ActivityScope |150| 0| 150|
|ScopedOrchestration |100| 100| 25|
|TransitiveOrchestration |100| 100| 25|
|TypedOrchestration |100| 0| 25|
|MainOrchestration |100| 0| 25|
|OrchestrationScope |200| 0| 200|

	