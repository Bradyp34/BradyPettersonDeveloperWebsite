PGDMP                      |            railway    16.4 (Debian 16.4-1.pgdg120+2)    17.1 &    C           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                           false            D           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                           false            E           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                           false            F           1262    16384    railway    DATABASE     r   CREATE DATABASE railway WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'en_US.utf8';
    DROP DATABASE railway;
                     postgres    false            �            1259    16399    feature    TABLE     �   CREATE TABLE public.feature (
    id integer NOT NULL,
    featurename text,
    description text,
    moscow integer NOT NULL,
    projectid integer NOT NULL
);
    DROP TABLE public.feature;
       public         heap r       postgres    false            �            1259    16398    feature_id_seq    SEQUENCE     �   CREATE SEQUENCE public.feature_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 %   DROP SEQUENCE public.feature_id_seq;
       public               postgres    false    218            G           0    0    feature_id_seq    SEQUENCE OWNED BY     A   ALTER SEQUENCE public.feature_id_seq OWNED BY public.feature.id;
          public               postgres    false    217            �            1259    16390    project    TABLE     e   CREATE TABLE public.project (
    id integer NOT NULL,
    projectname text,
    description text
);
    DROP TABLE public.project;
       public         heap r       postgres    false            �            1259    16389    project_id_seq    SEQUENCE     �   CREATE SEQUENCE public.project_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 %   DROP SEQUENCE public.project_id_seq;
       public               postgres    false    216            H           0    0    project_id_seq    SEQUENCE OWNED BY     A   ALTER SEQUENCE public.project_id_seq OWNED BY public.project.id;
          public               postgres    false    215            �            1259    16417    projecttask    TABLE     �   CREATE TABLE public.projecttask (
    id integer NOT NULL,
    taskname text NOT NULL,
    started date,
    due date,
    assigneeid integer,
    details text,
    stage integer,
    projectid integer NOT NULL
);
    DROP TABLE public.projecttask;
       public         heap r       postgres    false            �            1259    16416    projecttask_id_seq    SEQUENCE     �   CREATE SEQUENCE public.projecttask_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 )   DROP SEQUENCE public.projecttask_id_seq;
       public               postgres    false    222            I           0    0    projecttask_id_seq    SEQUENCE OWNED BY     I   ALTER SEQUENCE public.projecttask_id_seq OWNED BY public.projecttask.id;
          public               postgres    false    221            �            1259    24577    projectuser    TABLE     a   CREATE TABLE public.projectuser (
    projectid integer NOT NULL,
    userid integer NOT NULL
);
    DROP TABLE public.projectuser;
       public         heap r       postgres    false            �            1259    32769    projectuser_id_seq    SEQUENCE     {   CREATE SEQUENCE public.projectuser_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 )   DROP SEQUENCE public.projectuser_id_seq;
       public               postgres    false            �            1259    16408    siteuser    TABLE     �   CREATE TABLE public.siteuser (
    id integer NOT NULL,
    fullname text NOT NULL,
    username text NOT NULL,
    password text NOT NULL,
    "position" text
);
    DROP TABLE public.siteuser;
       public         heap r       postgres    false            �            1259    16407    siteuser_id_seq    SEQUENCE     �   CREATE SEQUENCE public.siteuser_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 &   DROP SEQUENCE public.siteuser_id_seq;
       public               postgres    false    220            J           0    0    siteuser_id_seq    SEQUENCE OWNED BY     C   ALTER SEQUENCE public.siteuser_id_seq OWNED BY public.siteuser.id;
          public               postgres    false    219            �            1259    16442    taskuser    TABLE     [   CREATE TABLE public.taskuser (
    taskid integer NOT NULL,
    userid integer NOT NULL
);
    DROP TABLE public.taskuser;
       public         heap r       postgres    false            �           2604    16402 
   feature id    DEFAULT     h   ALTER TABLE ONLY public.feature ALTER COLUMN id SET DEFAULT nextval('public.feature_id_seq'::regclass);
 9   ALTER TABLE public.feature ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    218    217    218            �           2604    16393 
   project id    DEFAULT     h   ALTER TABLE ONLY public.project ALTER COLUMN id SET DEFAULT nextval('public.project_id_seq'::regclass);
 9   ALTER TABLE public.project ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    216    215    216            �           2604    16420    projecttask id    DEFAULT     p   ALTER TABLE ONLY public.projecttask ALTER COLUMN id SET DEFAULT nextval('public.projecttask_id_seq'::regclass);
 =   ALTER TABLE public.projecttask ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    221    222    222            �           2604    16411    siteuser id    DEFAULT     j   ALTER TABLE ONLY public.siteuser ALTER COLUMN id SET DEFAULT nextval('public.siteuser_id_seq'::regclass);
 :   ALTER TABLE public.siteuser ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    220    219    220            9          0    16399    feature 
   TABLE DATA           R   COPY public.feature (id, featurename, description, moscow, projectid) FROM stdin;
    public               postgres    false    218   �'       7          0    16390    project 
   TABLE DATA           ?   COPY public.project (id, projectname, description) FROM stdin;
    public               postgres    false    216   �)       =          0    16417    projecttask 
   TABLE DATA           h   COPY public.projecttask (id, taskname, started, due, assigneeid, details, stage, projectid) FROM stdin;
    public               postgres    false    222   1+       ?          0    24577    projectuser 
   TABLE DATA           8   COPY public.projectuser (projectid, userid) FROM stdin;
    public               postgres    false    224   .       ;          0    16408    siteuser 
   TABLE DATA           P   COPY public.siteuser (id, fullname, username, password, "position") FROM stdin;
    public               postgres    false    220   F.       >          0    16442    taskuser 
   TABLE DATA           2   COPY public.taskuser (taskid, userid) FROM stdin;
    public               postgres    false    223   j/       K           0    0    feature_id_seq    SEQUENCE SET     =   SELECT pg_catalog.setval('public.feature_id_seq', 19, true);
          public               postgres    false    217            L           0    0    project_id_seq    SEQUENCE SET     =   SELECT pg_catalog.setval('public.project_id_seq', 11, true);
          public               postgres    false    215            M           0    0    projecttask_id_seq    SEQUENCE SET     A   SELECT pg_catalog.setval('public.projecttask_id_seq', 19, true);
          public               postgres    false    221            N           0    0    projectuser_id_seq    SEQUENCE SET     A   SELECT pg_catalog.setval('public.projectuser_id_seq', 17, true);
          public               postgres    false    225            O           0    0    siteuser_id_seq    SEQUENCE SET     >   SELECT pg_catalog.setval('public.siteuser_id_seq', 16, true);
          public               postgres    false    219            �           2606    16406    feature feature_pkey 
   CONSTRAINT     R   ALTER TABLE ONLY public.feature
    ADD CONSTRAINT feature_pkey PRIMARY KEY (id);
 >   ALTER TABLE ONLY public.feature DROP CONSTRAINT feature_pkey;
       public                 postgres    false    218            �           2606    16397    project project_pkey 
   CONSTRAINT     R   ALTER TABLE ONLY public.project
    ADD CONSTRAINT project_pkey PRIMARY KEY (id);
 >   ALTER TABLE ONLY public.project DROP CONSTRAINT project_pkey;
       public                 postgres    false    216            �           2606    16424    projecttask projecttask_pkey 
   CONSTRAINT     Z   ALTER TABLE ONLY public.projecttask
    ADD CONSTRAINT projecttask_pkey PRIMARY KEY (id);
 F   ALTER TABLE ONLY public.projecttask DROP CONSTRAINT projecttask_pkey;
       public                 postgres    false    222            �           2606    16415    siteuser siteuser_pkey 
   CONSTRAINT     T   ALTER TABLE ONLY public.siteuser
    ADD CONSTRAINT siteuser_pkey PRIMARY KEY (id);
 @   ALTER TABLE ONLY public.siteuser DROP CONSTRAINT siteuser_pkey;
       public                 postgres    false    220            9   *  x�}S�r�0<C_�So�D��:����L�x���^(	�9�Hv��))q/�� �],�3x�Pak�pd��x���Bu�rS��,��u�z�ܚ:(�g�d�'.����,�T�o�0�n�\��^:/��ሡ����N��F��'Ȼ�@���&�����~/���'Y�Z!Uj^l�W%rf� ��7�ScS��֓NX���W�H���ȉ�]d ˺�8�ƨI���X������9�.�z�ҢT��4,�I
|6�Q��kQQȋ9β|�����
P)sF�u,�h(-���ZM���~X�5	,��BM�l	�.ɟ�tZ�c��,_|����t��@o�̒$�����T
�1NW�X�ON6z:~���CQ(.���:���7mܴ�&(ѻ���*q��޻oF��o�#�J��n#g���XUE�ae��`���Ay9y�	Q�Q���H�������X��eŎ�gxI�+.�Sۓ�f�~z?h�F��|��*�p):�+o`�)�=���`�!�*1~���N
�ߊWnv���fY���N�      7   C  x�U��N!���)��X����1&nlZo���t�0��o/tR��9��9���l�d�zQ�s%��ASUV~�䍨ik,�c����$�=(Б�g��������
Aފj]���	�����a�12j�sg�ʫKG��d+�$�ū?���9<4�+�;�P�3��d� 6���PccT�,:��;I9�5�w]\�wo�GxC��-�(&eE�܏�i���aM���r��	V��Q'���0� !�#�SV#u&6#9�w�y�.ߠFX�b�7�ɓ�	��J�1���,���;0j�g_s)�4n��      =   �  x�mT�n�0=�_�h�I��(lX���e�f��dHr���Gʶ�m�11������=��]�[k~A鹅�8卽21�����<O��l��W�V���3L*��Ur�v��,̰$�
�� =�J���H[�]��oc(�L�-8u���Ԧ�:v��{�%�Ld��W��R��`E����!�hl O�G$���E�d���w�z��D�9r�5mhcC��T]X�-����������=�di���jR}�t�Ѩ���"[1�kcYs-���G�E�v�}r^��q�N��G�^���E7]`'~��c*b�|.��{d����~� �.�68��U���ȶ"�sl���?�gh�G�0�`�� ���e�ː\��_��1_pN�qʰf�\�(��"�d�ޢ�A�4\C�/���*ҽ9�ϳ|A׈Ǹ�}�l�^�P+��1_%F��1��N�T�Fn��\8����3v�
�D���{�Sx���:�݄m�?{�|$��Q�I'�^f襯�W=Q������TC�*��5���O�|�K�7�$B��);b,�Z.�1-w�ⲮC���KkF�5m��/���&�"��mB��hp�����!r���Q�< ��O��Ɍk��>K��3?P�����`�Lă��I��5
>Iqz0I��Z	g���
���yhf��β,�LK�#      ?   3   x���  ���{�f�� �T�jJ�V�b)M�ݶ[*�����Î�      ;     x����N�0���� ܔ�kJ
�ʡ.g�8�v�M��=v(�8�ی4��<�QI�;�o`vL�}���^�H��;rl��Z��Cc�O��Q~h�SJpף�р�q��� �J�5�2����a*�3){u6��l�/[����x��!�i�3�s>�2�����������#�&�w�2-?��dܜjP{�F�ܹ@׳������WccH:B\L^"WO$U�v��`��{�$�2[
��L�~��2���      >   A   x�=ʻ	 1�xU�ay�����q�B��U�*Y�T���7�c���6�r�I�7���$���     