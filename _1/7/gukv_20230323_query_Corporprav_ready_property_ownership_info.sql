SELECT 
uridname as [Назва юр. особи],
uridzkpo as [Код ЕДРПОУ юр. особи],
vlasname as [Назва / ФІО власника корпправа],
vlasicod as [ІНН власника корпправа],
vlaszkpo as [ЕДРПОУ власника корпправ],
vlaskopfg as [КОПФГ власника корп.права],
vlasaddr as [Юр. Адреса власника корп.права],
isvlas as [Є засно-вником],
akckol as [Кількість акцій у власника, шт.],
akcvart as [Частка власника, грн.],
akcpart as [Частка власника, %],
akcform as [Форма існування акцій],
ownershipvid as [Форма власності],
nomvart as [Номінальна вартість акцій, грн],
stanobj as [Стан реєстрації об'єкту корпоративної власності],
modify_date2 as [Дата редагу-вання],
modified_by2 as [Користувач]
FROM [corporprav] A
