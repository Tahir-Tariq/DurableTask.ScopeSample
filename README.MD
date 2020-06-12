# Durable scope table



|(First Orchestrator scope)	  |TypedOrchestration	   |TransitiveOrchestration	   |ScopedOrchestration     |
|------------------------------|------------------------|---------------------------|-----------------------	|
|TypedActivity *1|**Per request**|**Per request**|**Per request**|
|TransitiveActivity|Per request retained|Per request retained|Per request retained|
|ScopedActivity|Singleton|Singleton|Singleton|
|TypedSubOrchestration|Per request	|Per request|Per request|Per request|
|TransitiveSubOrchestration|Per request retained|Per request but retained|Per request but retained|
|ScopedSubOrchestration|Singleton|Singleton|Singleton|
|ProxyActivity(as Transitive)|Per request retained|Per request but retained|Per request but retained|
|ProxyActivity(as Scoped)|Singleton|Singleton|Singleton|
|New scope inside ServiceProviderObjectCreator.Create method.|
|TypedActivity|Per request|Per request|Per request|
|TransitiveActivity*2|**Per request**|**Per request**|**Per request**|
|ScopedActivity	|Per request|Singleton|Singleton|
|TypedSubOrchestration|Per request|Per request|Per request|
|TransitiveSubOrchestration *1|Per request|Per request|Per request|
|ScopedSubOrchestration|Per request|Per request|Per request|
|ProxyActivity(as Transitive)|Per request|Per request|Per request|
|ProxyActivity(as Scoped)|Per request|Per request|Per request|

* Per request but retaine: A new object is created every time but then it is not disposed after the use.
* 1: DurableTaskFramework handling of object creation and removal. (This is what we aiming for with DI).
* 2: Custom object creation handling with dependency injection that is creating same object lifetime as of DurableFramework.

MainOrchestration executed as typed orchestration
----------------------------------------------------------------------------
Finalizer: TypedActivity: 73f0a3c8-e950-45c2-a472-9c7c88c309e3
Finalizer: TypedActivity: 6c0d38f1-7889-495a-b170-6f6e1287a6db
Finalizer: TransitiveActivity: 2277f248-ea48-4afe-82e3-a66af936c5ae
Finalizer: TransitiveActivity: 15ca4a54-3453-44fb-98e1-5d7f64da066a
Finalizer: TransitiveActivity: 4ae20b7d-fef3-4f12-ba12-29fa5beace10
Finalizer: TypedActivity: 0e814adc-27fc-4536-89c9-0b354d6d6f0d
Finalizer: TypedActivity: 7837e297-0e51-421c-90fe-6b13ae2266b2
Finalizer: TransitiveActivity: a1427b6c-f214-4a35-a1f1-98c498acf33d
Finalizer: TypedActivity: ebefb6f6-7bcf-4b00-b715-54b086bc6fed
Finalizer: TypedActivity: 2fb41e2d-12f3-486b-99e4-4111dbd60300

Request1MainOrchestration: 4bec2573-0cb6-4402-ace4-888dcdde649f TransitiveOrchestration: 71988703-494d-49a2-b69f-597fdd2c92cc [TypedActivity: 6c0d38f1-7889-495a-b170-6f6e1287a6db ScopedActivity: a9a9385f-68af-47d2-8b92-a01b21ebaf3f TransitiveActivity: 2277f248-ea48-4afe-82e3-a66af936c5ae ]ScopedOrchestration: df143b99-505a-4794-938b-bbeda8d95d10 [TypedActivity: 0e814adc-27fc-4536-89c9-0b354d6d6f0d ScopedActivity: a9a9385f-68af-47d2-8b92-a01b21ebaf3f TransitiveActivity: 4ae20b7d-fef3-4f12-ba12-29fa5beace10 ]TypedOrchestration: 20769483-f704-4d22-b400-74aedc8533a9 [TypedActivity: 2fb41e2d-12f3-486b-99e4-4111dbd60300 ScopedActivity: a9a9385f-68af-47d2-8b92-a01b21ebaf3f TransitiveActivity: 6ea7e9a6-6639-4966-9545-33c91ec36167 ]

Request2MainOrchestration: 80e17a08-9709-46b5-be77-6b3597d37c3f TransitiveOrchestration: 4e3616f0-bf02-4546-861e-b5bbe0558ad5 [TypedActivity: 73f0a3c8-e950-45c2-a472-9c7c88c309e3 ScopedActivity: a9a9385f-68af-47d2-8b92-a01b21ebaf3f TransitiveActivity: 15ca4a54-3453-44fb-98e1-5d7f64da066a ]ScopedOrchestration: df143b99-505a-4794-938b-bbeda8d95d10 [TypedActivity: 7837e297-0e51-421c-90fe-6b13ae2266b2 ScopedActivity: a9a9385f-68af-47d2-8b92-a01b21ebaf3f TransitiveActivity: a1427b6c-f214-4a35-a1f1-98c498acf33d ]TypedOrchestration: 246dee18-ac22-4826-afa5-d125201f7b99 [TypedActivity: ebefb6f6-7bcf-4b00-b715-54b086bc6fed ScopedActivity: a9a9385f-68af-47d2-8b92-a01b21ebaf3f TransitiveActivity: 4b6d9a38-ca1e-4051-b08c-dd88910db87e ]


Memory used before collection:       12,492,288
Finalizer: TypedOrchestration: 20769483-f704-4d22-b400-74aedc8533a9
Finalizer: TransitiveActivity: 4b6d9a38-ca1e-4051-b08c-dd88910db87e
Finalizer: TypedOrchestration: 246dee18-ac22-4826-afa5-d125201f7b99
Finalizer: TransitiveActivity: 6ea7e9a6-6639-4966-9545-33c91ec36167
Finalizer: MainOrchestration: 80e17a08-9709-46b5-be77-6b3597d37c3f
Finalizer: MainOrchestration: 4bec2573-0cb6-4402-ace4-888dcdde649f
Memory used after full collection:   3,315,992
Task hub is stopped


MainOrchestration executed as transitive orchestration
----------------------------------------------------------------------------
Finalizer: TypedActivity: 9797bef3-fdf1-4834-9443-0dde805c5148
Finalizer: TypedActivity: 602e1646-7bac-4328-97ee-edee44696d65
Finalizer: TransitiveActivity: 943437c1-bd78-4f2d-8b7f-5a83268156b8
Finalizer: TransitiveActivity: 3f99b4a1-9de1-42f7-a363-e4a6c5e374ef
Finalizer: TypedActivity: d246286b-6926-45c6-8c29-a1d3349bfee2
Finalizer: TypedActivity: d95c4316-c2ac-4bb2-aa4f-568a096131ca
Finalizer: TransitiveActivity: 13735313-523d-4667-abdc-c186a042a2fe
Finalizer: TransitiveActivity: 86f12123-b817-408c-9314-95927cc976af
Finalizer: TypedActivity: 064456ab-6b39-4f97-849d-9c32db567c15
Finalizer: TypedActivity: b3640d54-5cfd-4f24-807f-27eca2133f9e

Request2MainOrchestration: 4914ac29-79c5-4458-826f-88cf7b496537 TransitiveOrchestration: 7b08773b-40a2-4e30-ba30-0ed9538e23b2 [TypedActivity: 9797bef3-fdf1-4834-9443-0dde805c5148 ScopedActivity: e138e5e8-105b-4e2d-a49c-9c4e97274e1b TransitiveActivity: 3f99b4a1-9de1-42f7-a363-e4a6c5e374ef ]ScopedOrchestration: c3696629-6516-4f22-ba03-deb7e4d1f257 [TypedActivity: d246286b-6926-45c6-8c29-a1d3349bfee2 ScopedActivity: e138e5e8-105b-4e2d-a49c-9c4e97274e1b TransitiveActivity: 86f12123-b817-408c-9314-95927cc976af ]TypedOrchestration: 09ab5d17-e6ce-4224-8189-472c2d2a3260 [TypedActivity: b3640d54-5cfd-4f24-807f-27eca2133f9e ScopedActivity: e138e5e8-105b-4e2d-a49c-9c4e97274e1b TransitiveActivity: 79148741-3dbf-4d98-9672-11f287f3576b ]
Request1MainOrchestration: 5fa37888-d459-4be8-abea-b48e2437f3ae TransitiveOrchestration: b917a769-8ea8-41d4-8c4d-87cfd8890965 [TypedActivity: 602e1646-7bac-4328-97ee-edee44696d65 ScopedActivity: e138e5e8-105b-4e2d-a49c-9c4e97274e1b TransitiveActivity: 943437c1-bd78-4f2d-8b7f-5a83268156b8 ]ScopedOrchestration: c3696629-6516-4f22-ba03-deb7e4d1f257 [TypedActivity: d95c4316-c2ac-4bb2-aa4f-568a096131ca ScopedActivity: e138e5e8-105b-4e2d-a49c-9c4e97274e1b TransitiveActivity: 13735313-523d-4667-abdc-c186a042a2fe ]TypedOrchestration: 4aafe1d6-95a1-41ce-bd3e-98246048b226 [TypedActivity: 064456ab-6b39-4f97-849d-9c32db567c15 ScopedActivity: e138e5e8-105b-4e2d-a49c-9c4e97274e1b TransitiveActivity: 444ec138-1099-4ee1-bd2a-1778f13abcac ]

Finalizer: TypedOrchestration: 4aafe1d6-95a1-41ce-bd3e-98246048b226
Finalizer: TransitiveActivity: 79148741-3dbf-4d98-9672-11f287f3576b
Finalizer: TransitiveActivity: 444ec138-1099-4ee1-bd2a-1778f13abcac
Finalizer: TypedOrchestration: 09ab5d17-e6ce-4224-8189-472c2d2a3260

Memory used before collection:       5,941,432
Memory used after full collection:   3,364,840
Task hub is stopped


MainOrchestration executed as scoped orchestration
----------------------------------------------------------------------------
Finalizer: TypedActivity: 1730f44e-e55f-40db-b745-9a78df70897d
Finalizer: TypedActivity: 94b18882-aeed-4d49-9c02-30d10a19168a
Finalizer: TransitiveActivity: 9947c584-ffb3-47f8-a686-083b312a2717
Finalizer: TransitiveActivity: b9f4b26b-4423-4c6b-bc6f-a9309870d02f
Finalizer: TypedActivity: 03d9553c-4d81-409d-b390-a19378d68524
Finalizer: TypedActivity: f2286755-a708-4b4a-97a6-18078e929a49
Finalizer: TypedActivity: 815bd368-10f7-4445-8b2a-51f146474e09
Finalizer: TypedActivity: ec1e408b-fa25-4957-b5da-09b7c15d7ec4
Finalizer: TransitiveActivity: e428c0f3-02ce-4763-a7c0-826d6ce06d0e
Finalizer: TransitiveActivity: 96243e53-7718-43b8-bb65-daae6dd8a1fa

Request2MainOrchestration: 6a355703-fba8-490e-815b-2548d5d3e56a TransitiveOrchestration: 49f4bfb4-6dcc-482a-8458-ce1802c99ec7 [TypedActivity: 1730f44e-e55f-40db-b745-9a78df70897d ScopedActivity: 194f26fe-fe03-4b2b-8f5f-7636f3287d11 TransitiveActivity: b9f4b26b-4423-4c6b-bc6f-a9309870d02f ]ScopedOrchestration: 7645b27a-f950-40ab-95ae-3a1121e9c26a [TypedActivity: 03d9553c-4d81-409d-b390-a19378d68524 ScopedActivity: 194f26fe-fe03-4b2b-8f5f-7636f3287d11 TransitiveActivity: 96243e53-7718-43b8-bb65-daae6dd8a1fa ]TypedOrchestration: 29bdde13-df00-4c66-8674-fc3e379bd0fc [TypedActivity: 815bd368-10f7-4445-8b2a-51f146474e09 ScopedActivity: 194f26fe-fe03-4b2b-8f5f-7636f3287d11 TransitiveActivity: e5eba1a4-c586-4a20-b29b-0a027167ef50 ]

Request1MainOrchestration: 6a355703-fba8-490e-815b-2548d5d3e56a TransitiveOrchestration: 19775ebf-18cf-4a78-a3cf-cd9bc5cc85db [TypedActivity: 94b18882-aeed-4d49-9c02-30d10a19168a ScopedActivity: 194f26fe-fe03-4b2b-8f5f-7636f3287d11 TransitiveActivity: 9947c584-ffb3-47f8-a686-083b312a2717 ]ScopedOrchestration: 7645b27a-f950-40ab-95ae-3a1121e9c26a [TypedActivity: f2286755-a708-4b4a-97a6-18078e929a49 ScopedActivity: 194f26fe-fe03-4b2b-8f5f-7636f3287d11 TransitiveActivity: e428c0f3-02ce-4763-a7c0-826d6ce06d0e ]TypedOrchestration: dd39ad52-ce28-4039-8c09-66552afd1598 [TypedActivity: ec1e408b-fa25-4957-b5da-09b7c15d7ec4 ScopedActivity: 194f26fe-fe03-4b2b-8f5f-7636f3287d11 TransitiveActivity: 09ae7113-bbf2-45b2-8230-841eba147e87 ]
Finalizer: TypedOrchestration: dd39ad52-ce28-4039-8c09-66552afd1598
Finalizer: TransitiveActivity: e5eba1a4-c586-4a20-b29b-0a027167ef50
Finalizer: TransitiveActivity: 09ae7113-bbf2-45b2-8230-841eba147e87
Finalizer: TypedOrchestration: 29bdde13-df00-4c66-8674-fc3e379bd0fc

Memory used before collection:       8,871,440
Memory used after full collection:   3,317,352
Task hub is stopped


MainOrchestration executed as typed orchestration and creating new scope in ServiceProviderObjectCreator.Create method.
----------------------------------------------------------------------------
Finalizer: TypedActivity: 90eecf97-357c-4dd3-a306-aa7a77b9653a
Finalizer: TypedActivity: 269fe1c4-7904-4959-be75-a7cc8eba23df
Finalizer: ScopedActivity: 03d531fa-f0ca-4bf6-9c6d-318b69a91a11
Finalizer: TransitiveActivity: 8806c1d9-e244-4595-9a1a-5e47bebe8c48
Finalizer: ScopedActivity: cd65d3f9-1f86-4e5a-b289-d95465e40abe
Finalizer: TransitiveActivity: 4e6f0575-43a6-45ed-9e7d-defb312bee56
Finalizer: TransitiveOrchestration: 0cf122ea-00e3-4667-bda4-11411687bf3d
Finalizer: TransitiveOrchestration: 6b040422-ddad-4d69-a590-2175f31b58e4
Finalizer: TransitiveActivity: d5342577-608a-40a2-9abe-7d674f064a25
Finalizer: ScopedActivity: e7fb5b06-16c3-41a3-b785-ef9809ba5db5
Finalizer: ScopedActivity: 96611100-7d86-4d21-ad03-70a6e4b3e57a
Finalizer: TypedActivity: fde7a0c5-3532-474f-b840-95c0396c132e
Finalizer: TransitiveActivity: 552baad6-ea20-4434-814a-71bbec0c88d1
Finalizer: TypedActivity: 27be79b4-78c3-4703-9088-84ce967ead30
Finalizer: TypedActivity: 0440bbfd-49d0-4f1a-81d0-eade0f362bc7
Finalizer: ScopedOrchestration: 74851745-f557-4db2-9494-a0298e30c482
Finalizer: ScopedOrchestration: 327bfa0e-0702-410e-b32b-3e4900bdcc54
Finalizer: TypedActivity: 5e1ac063-9eae-4bc6-931c-f3ce4c2de1cc
Finalizer: ScopedActivity: ff55cde2-e39f-4b8d-8188-0f50fb360b22
Finalizer: ScopedActivity: 8c69fe95-f0d5-4e7c-8de4-35d755f132d9

Request1MainOrchestration: af54dcbc-1f43-4fc9-8432-784ca29562bb TransitiveOrchestration: 0cf122ea-00e3-4667-bda4-11411687bf3d [TypedActivity: 269fe1c4-7904-4959-be75-a7cc8eba23df ScopedActivity: cd65d3f9-1f86-4e5a-b289-d95465e40abe TransitiveActivity: 8806c1d9-e244-4595-9a1a-5e47bebe8c48 ]ScopedOrchestration: 327bfa0e-0702-410e-b32b-3e4900bdcc54 [TypedActivity: 27be79b4-78c3-4703-9088-84ce967ead30 ScopedActivity: 96611100-7d86-4d21-ad03-70a6e4b3e57a TransitiveActivity: d5342577-608a-40a2-9abe-7d674f064a25 ]TypedOrchestration: 9ae40f7a-5fe9-4a88-b29b-893bc3cef7f5 [TypedActivity: 5e1ac063-9eae-4bc6-931c-f3ce4c2de1cc ScopedActivity: ff55cde2-e39f-4b8d-8188-0f50fb360b22 TransitiveActivity: 89e589e7-afc1-435f-b94a-9450459666f2 ]

Request2MainOrchestration: 22ff3711-99f2-475e-bcca-fbae44fde2b0 TransitiveOrchestration: 6b040422-ddad-4d69-a590-2175f31b58e4 [TypedActivity: 90eecf97-357c-4dd3-a306-aa7a77b9653a ScopedActivity: 03d531fa-f0ca-4bf6-9c6d-318b69a91a11 TransitiveActivity: 4e6f0575-43a6-45ed-9e7d-defb312bee56 ]ScopedOrchestration: 74851745-f557-4db2-9494-a0298e30c482 [TypedActivity: fde7a0c5-3532-474f-b840-95c0396c132e ScopedActivity: e7fb5b06-16c3-41a3-b785-ef9809ba5db5 TransitiveActivity: 552baad6-ea20-4434-814a-71bbec0c88d1 ]TypedOrchestration: 47c82f46-a0fb-46f5-aef8-d10672090d4d [TypedActivity: 0440bbfd-49d0-4f1a-81d0-eade0f362bc7 ScopedActivity: 8c69fe95-f0d5-4e7c-8de4-35d755f132d9 TransitiveActivity: b882c7b1-dc0c-4d1d-af45-5c8f1368a652 ]

Finalizer: MainOrchestration: af54dcbc-1f43-4fc9-8432-784ca29562bb
Finalizer: TypedOrchestration: 9ae40f7a-5fe9-4a88-b29b-893bc3cef7f5
Finalizer: TypedOrchestration: 47c82f46-a0fb-46f5-aef8-d10672090d4d
Finalizer: TransitiveActivity: b882c7b1-dc0c-4d1d-af45-5c8f1368a652
Finalizer: MainOrchestration: 22ff3711-99f2-475e-bcca-fbae44fde2b0
Finalizer: TransitiveActivity: 89e589e7-afc1-435f-b94a-9450459666f2

Memory used before collection:       6,047,152
Memory used after full collection:   3,272,264
Task hub is stopped




MainOrchestration executed as transitive orchestration and creating new scope in ServiceProviderObjectCreator.Create method.
----------------------------------------------------------------------------
Finalizer: TypedActivity: e241e382-dfd3-49e7-ab5c-5b8a1be2e8af
Finalizer: TypedActivity: f54a2ad7-2ace-42ac-b586-a13d20812d14
Finalizer: ScopedActivity: 7b2e1efb-e6bd-44b0-af12-d87274adac44
Finalizer: ScopedActivity: 3da317e4-5bfe-427a-91df-659667ee304a
Finalizer: TransitiveActivity: d320bd2b-a480-4ae7-8d4d-b8f9c55631b5
Finalizer: TransitiveOrchestration: a2dde691-bd3f-43fd-b0fe-e086c52d0d11
Finalizer: TransitiveActivity: 3282772f-f5b3-4721-bd19-c13894ef9885
Finalizer: TransitiveOrchestration: 14780527-c796-47cd-820e-537a26652db8
Finalizer: TransitiveActivity: 640efbcc-f57a-433b-a28b-536b9a371410
Finalizer: TransitiveActivity: 6ce8bb21-76fc-4b03-8e99-537972f30617
Finalizer: TypedActivity: 9fe708d8-879f-4e28-a2a0-6cd34aa307c0
Finalizer: TypedActivity: 6cbc3a22-0b9f-4042-b5f4-8f9958bf308a
Finalizer: ScopedActivity: fca637ae-47b2-43c6-addc-1fe2977e4938
Finalizer: ScopedActivity: f4ac9662-e404-4330-9aa9-a3059be3d11f
Finalizer: TypedActivity: 1c2ce421-75ae-4cc7-98ea-a96f114d5b68
Finalizer: TypedActivity: e8b8648c-a61f-4691-a34b-c91fadd51360
Finalizer: ScopedActivity: ce35f515-5a3d-4410-b7b7-0b0cc9cfa270
Finalizer: ScopedOrchestration: 40ade289-48f0-4172-a135-e5f1fce4decc
Request1MainOrchestration: 21adea9a-9f0d-4859-83e0-8be1a8969423 TransitiveOrchestration: a2dde691-bd3f-43fd-b0fe-e086c52d0d11 [TypedActivity: f54a2ad7-2ace-42ac-b586-a13d20812d14 ScopedActivity: 3da317e4-5bfe-427a-91df-659667ee304a TransitiveActivity: d320bd2b-a480-4ae7-8d4d-b8f9c55631b5 ]ScopedOrchestration: 40ade289-48f0-4172-a135-e5f1fce4decc [TypedActivity: 6cbc3a22-0b9f-4042-b5f4-8f9958bf308a ScopedActivity: f4ac9662-e404-4330-9aa9-a3059be3d11f TransitiveActivity: 640efbcc-f57a-433b-a28b-536b9a371410 ]TypedOrchestration: b4eff7de-feb8-4f77-93e5-933709b7a92a [TypedActivity: e8b8648c-a61f-4691-a34b-c91fadd51360 ScopedActivity: ce35f515-5a3d-4410-b7b7-0b0cc9cfa270 TransitiveActivity: 14e272da-6526-42f9-bec8-9b3ad71d8246 ]
Request2MainOrchestration: a5d46ae4-c561-45f4-9c7e-fbac8e8d02ef TransitiveOrchestration: 14780527-c796-47cd-820e-537a26652db8 [TypedActivity: e241e382-dfd3-49e7-ab5c-5b8a1be2e8af ScopedActivity: 7b2e1efb-e6bd-44b0-af12-d87274adac44 TransitiveActivity: 3282772f-f5b3-4721-bd19-c13894ef9885 ]ScopedOrchestration: 97dcb805-32c0-462f-b3de-ca79eefeb268 [TypedActivity: 9fe708d8-879f-4e28-a2a0-6cd34aa307c0 ScopedActivity: fca637ae-47b2-43c6-addc-1fe2977e4938 TransitiveActivity: 6ce8bb21-76fc-4b03-8e99-537972f30617 ]TypedOrchestration: 6fe6bad4-341f-4ad3-b200-d391c3dfa769 [TypedActivity: 1c2ce421-75ae-4cc7-98ea-a96f114d5b68 ScopedActivity: d4920a48-133e-4e9b-98fa-d4432d8b28b7 TransitiveActivity: d7c549f8-761e-4a2d-a5ef-121b86e9a2f7 ]
Finalizer: TransitiveActivity: d7c549f8-761e-4a2d-a5ef-121b86e9a2f7
Finalizer: TransitiveActivity: 14e272da-6526-42f9-bec8-9b3ad71d8246
Finalizer: ScopedActivity: d4920a48-133e-4e9b-98fa-d4432d8b28b7
Finalizer: ScopedOrchestration: 97dcb805-32c0-462f-b3de-ca79eefeb268
Finalizer: TypedOrchestration: 6fe6bad4-341f-4ad3-b200-d391c3dfa769
Finalizer: TypedOrchestration: b4eff7de-feb8-4f77-93e5-933709b7a92a
Finalizer: MainOrchestration: 21adea9a-9f0d-4859-83e0-8be1a8969423
Finalizer: MainOrchestration: a5d46ae4-c561-45f4-9c7e-fbac8e8d02ef

Memory used before collection:       7,828,592
Memory used after full collection:   3,280,136
Task hub is stopped


MainOrchestration executed as scoped orchestration and creating new scope in ServiceProviderObjectCreator.Create method.
----------------------------------------------------------------------------
Finalizer: TypedActivity: 6997459f-b9ca-4a6e-97f3-5b23d5e3403c
Finalizer: TypedActivity: 8948be99-5c11-41c8-b7a7-c8b306a374c4
Finalizer: TransitiveActivity: 0fc801d3-5058-4fcb-a2df-1951da669e32
Finalizer: ScopedActivity: c662ba4c-7100-4f51-b301-4cdfd1eae850
Finalizer: TransitiveActivity: 2fd56b20-9353-4266-954b-17def8197566
Finalizer: ScopedActivity: 4e309957-7220-478e-9ff3-198009232d69
Finalizer: TypedActivity: 7dd65ecf-989d-4be3-b1ab-9b05a6a5355b
Finalizer: ScopedActivity: 22e56ea0-17b3-405d-bd33-448b8bc7bb9e
Finalizer: TypedActivity: 6e112f66-3f47-4498-b6e0-6c0926e7cf42
Finalizer: ScopedActivity: 043e6f92-2dd8-4759-8739-7bf0b46de62e
Finalizer: TypedActivity: c6fc2d60-cea8-4673-bf6a-35728b618510
Finalizer: TransitiveActivity: 30677194-b064-424f-b296-88fc80cfa748
Finalizer: ScopedOrchestration: 964bcd82-eea7-4b8e-8175-c3d816740fb1
Finalizer: TypedActivity: 7458be67-63a4-4f3a-be2b-eef456e5c472
Request1MainOrchestration: 4846579d-cf90-4912-9053-c01821a9d19e TransitiveOrchestration: 700278e8-2192-400f-964f-e804d2bdde78 [TypedActivity: 8948be99-5c11-41c8-b7a7-c8b306a374c4 ScopedActivity: 4e309957-7220-478e-9ff3-198009232d69 TransitiveActivity: 2fd56b20-9353-4266-954b-17def8197566 ]ScopedOrchestration: 780105a6-6cc7-4c99-9ff9-5b18c523e4eb [TypedActivity: 7dd65ecf-989d-4be3-b1ab-9b05a6a5355b ScopedActivity: 043e6f92-2dd8-4759-8739-7bf0b46de62e TransitiveActivity: ca4d28e4-bcf1-4f85-b4a6-d6af48d30e78 ]TypedOrchestration: d965f452-1175-4702-afd9-ea91b4018df9 [TypedActivity: 7458be67-63a4-4f3a-be2b-eef456e5c472 ScopedActivity: a8c7016d-7e77-469f-98a0-34d52e9ed140 TransitiveActivity: 27698a5a-808c-4ac0-8379-e769d9064be8 ]
Request2MainOrchestration: 4ab224d5-e5be-4099-9bb5-5f7a9e3189ef TransitiveOrchestration: c9445b01-b129-4d59-ad4d-7004427c2f61 [TypedActivity: 6997459f-b9ca-4a6e-97f3-5b23d5e3403c ScopedActivity: c662ba4c-7100-4f51-b301-4cdfd1eae850 TransitiveActivity: 0fc801d3-5058-4fcb-a2df-1951da669e32 ]ScopedOrchestration: 964bcd82-eea7-4b8e-8175-c3d816740fb1 [TypedActivity: 6e112f66-3f47-4498-b6e0-6c0926e7cf42 ScopedActivity: 22e56ea0-17b3-405d-bd33-448b8bc7bb9e TransitiveActivity: 30677194-b064-424f-b296-88fc80cfa748 ]TypedOrchestration: 07161753-e3ee-4408-b300-3e9e32eb06f0 [TypedActivity: c6fc2d60-cea8-4673-bf6a-35728b618510 ScopedActivity: 67b86e52-3ff6-4820-842f-bcc2307711e6 TransitiveActivity: 18d3a9fc-2ab6-4071-873a-edfae859f82c ]
Finalizer: TransitiveActivity: 27698a5a-808c-4ac0-8379-e769d9064be8
Finalizer: TypedOrchestration: d965f452-1175-4702-afd9-ea91b4018df9
Finalizer: TypedOrchestration: 07161753-e3ee-4408-b300-3e9e32eb06f0
Finalizer: ScopedActivity: 67b86e52-3ff6-4820-842f-bcc2307711e6
Finalizer: TransitiveActivity: 18d3a9fc-2ab6-4071-873a-edfae859f82c
Finalizer: ScopedActivity: a8c7016d-7e77-469f-98a0-34d52e9ed140

Memory used before collection:       8,878,456
Finalizer: TransitiveOrchestration: 700278e8-2192-400f-964f-e804d2bdde78
Finalizer: TransitiveOrchestration: c9445b01-b129-4d59-ad4d-7004427c2f61
Finalizer: MainOrchestration: 4846579d-cf90-4912-9053-c01821a9d19e
Finalizer: MainOrchestration: 4ab224d5-e5be-4099-9bb5-5f7a9e3189ef
Finalizer: ScopedOrchestration: 780105a6-6cc7-4c99-9ff9-5b18c523e4eb
Finalizer: TransitiveActivity: ca4d28e4-bcf1-4f85-b4a6-d6af48d30e78
Memory used after full collection:   3,341,984
Task hub is stopped
