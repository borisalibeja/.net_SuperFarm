--
-- PostgreSQL database dump
--

-- Dumped from database version 17.5
-- Dumped by pg_dump version 17.5

-- Started on 2025-07-19 13:28:48

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 221 (class 1255 OID 16595)
-- Name: set_farm_updated_at(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.set_farm_updated_at() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    NEW.farm_updated_at = CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$$;


ALTER FUNCTION public.set_farm_updated_at() OWNER TO postgres;

--
-- TOC entry 220 (class 1255 OID 16600)
-- Name: set_product_updated_at(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.set_product_updated_at() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    NEW.product_updated_at = CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$$;


ALTER FUNCTION public.set_product_updated_at() OWNER TO postgres;

--
-- TOC entry 222 (class 1255 OID 16597)
-- Name: set_user_updated_at(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.set_user_updated_at() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    NEW.user_updated_at = CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$$;


ALTER FUNCTION public.set_user_updated_at() OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 218 (class 1259 OID 16548)
-- Name: farms; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.farms (
    farm_id uuid NOT NULL,
    user_id uuid NOT NULL,
    farm_name character varying(100) NOT NULL,
    farm_created_at timestamp with time zone DEFAULT CURRENT_TIMESTAMP,
    street_name character varying(255),
    building_nr character varying(50),
    city character varying(100),
    postcode character varying(20),
    country character varying(100),
    county character varying(100),
    profile_image_url character varying(2083),
    cover_image_url character varying(2083),
    farm_phone_nr character varying(20),
    country_code character varying(5),
    farm_updated_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    farm_about text,
    farm_email character varying(100)
);


ALTER TABLE public.farms OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 16561)
-- Name: products; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.products (
    product_id uuid NOT NULL,
    farm_id uuid NOT NULL,
    product_name character varying(100) NOT NULL,
    product_price numeric(10,2),
    product_category character varying(100) NOT NULL,
    product_created_at timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    currency character varying(3) DEFAULT 'USD'::character varying NOT NULL,
    stock_unit character varying(50),
    stock_weight numeric(10,2),
    product_description text,
    image_url_1 character varying(2083),
    image_url_2 character varying(2083),
    image_url_3 character varying(2083),
    image_url_4 character varying(2083),
    image_url_5 character varying(2083),
    product_updated_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP
);


ALTER TABLE public.products OWNER TO postgres;

--
-- TOC entry 217 (class 1259 OID 16539)
-- Name: users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users (
    user_id uuid NOT NULL,
    user_name character varying(100) NOT NULL,
    password character varying(100) NOT NULL,
    first_name character varying(100) NOT NULL,
    last_name character varying(100) NOT NULL,
    age integer,
    user_email character varying(100) NOT NULL,
    user_phone_nr character varying(20),
    role character varying(50) NOT NULL,
    refresh_token character varying(256),
    refresh_token_expiry_time timestamp with time zone,
    street_name character varying(255),
    building_nr character varying(50),
    floor_nr character varying(20),
    city character varying(100),
    postcode character varying(20),
    country character varying(100),
    county character varying(100),
    user_created_at timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    profile_img_url character varying(2083),
    country_code character varying(5),
    user_updated_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.users OWNER TO postgres;

--
-- TOC entry 4764 (class 2606 OID 16555)
-- Name: farms farms_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.farms
    ADD CONSTRAINT farms_pkey PRIMARY KEY (farm_id);


--
-- TOC entry 4766 (class 2606 OID 16565)
-- Name: products products_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.products
    ADD CONSTRAINT products_pkey PRIMARY KEY (product_id);


--
-- TOC entry 4760 (class 2606 OID 16547)
-- Name: users users_email_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_email_key UNIQUE (user_email);


--
-- TOC entry 4762 (class 2606 OID 16545)
-- Name: users users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (user_id);


--
-- TOC entry 4770 (class 2620 OID 16596)
-- Name: farms trigger_set_farm_updated_at; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER trigger_set_farm_updated_at BEFORE UPDATE ON public.farms FOR EACH ROW EXECUTE FUNCTION public.set_farm_updated_at();


--
-- TOC entry 4771 (class 2620 OID 16601)
-- Name: products trigger_set_product_updated_at; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER trigger_set_product_updated_at BEFORE UPDATE ON public.products FOR EACH ROW EXECUTE FUNCTION public.set_product_updated_at();


--
-- TOC entry 4769 (class 2620 OID 16598)
-- Name: users trigger_set_user_updated_at; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER trigger_set_user_updated_at BEFORE UPDATE ON public.users FOR EACH ROW EXECUTE FUNCTION public.set_user_updated_at();


--
-- TOC entry 4768 (class 2606 OID 16566)
-- Name: products fk_farm; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.products
    ADD CONSTRAINT fk_farm FOREIGN KEY (farm_id) REFERENCES public.farms(farm_id) ON DELETE CASCADE;


--
-- TOC entry 4767 (class 2606 OID 16556)
-- Name: farms fk_user; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.farms
    ADD CONSTRAINT fk_user FOREIGN KEY (user_id) REFERENCES public.users(user_id) ON DELETE CASCADE;


-- Completed on 2025-07-19 13:28:48

--
-- PostgreSQL database dump complete
--

