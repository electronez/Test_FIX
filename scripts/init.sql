CREATE TABLE public.rate_histories (
	from_currency   varchar(3) NOT NULL,
	to_currency     varchar(3) NOT NULL,
	invalidate_date timestamp  NOT NULL,
	rate            numeric NULL,
	CONSTRAINT rate_histories_pk PRIMARY KEY (from_currency, to_currency, invalidate_date)
);

-- Column comments

COMMENT ON COLUMN public.rate_histories.from_currency IS 'Отдаваемая валюта';
COMMENT ON COLUMN public.rate_histories.to_currency IS 'Получаемая валюта';
COMMENT ON COLUMN public.rate_histories.invalidate_date IS 'Дата невалидности';
COMMENT ON COLUMN public.rate_histories.rate IS 'Значение';