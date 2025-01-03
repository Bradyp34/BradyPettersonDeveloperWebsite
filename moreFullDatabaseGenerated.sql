PGDMP  $                    |            railway    16.4 (Debian 16.4-1.pgdg120+2)    17.1 &    C           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
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
    public               postgres    false    216   
+       =          0    16417    projecttask 
   TABLE DATA           h   COPY public.projecttask (id, taskname, started, due, assigneeid, details, stage, projectid) FROM stdin;
    public               postgres    false    222   �+       ?          0    24577    projectuser 
   TABLE DATA           8   COPY public.projectuser (projectid, userid) FROM stdin;
    public               postgres    false    224   �3       ;          0    16408    siteuser 
   TABLE DATA           P   COPY public.siteuser (id, fullname, username, password, "position") FROM stdin;
    public               postgres    false    220   �3       >          0    16442    taskuser 
   TABLE DATA           2   COPY public.taskuser (taskid, userid) FROM stdin;
    public               postgres    false    223   5       K           0    0    feature_id_seq    SEQUENCE SET     =   SELECT pg_catalog.setval('public.feature_id_seq', 19, true);
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
       public                 postgres    false    220            9   V  x�]T�r�8<�_1?�TD�I|T$e�U�f�Rr�&�$� � ���� E�kO$��yt�`-�ct����Q�����?G�5{���~�Hf���j��!g���1�X�uQ��9���2t/Nz%�Y�U�=S���@�w=5F�]�0��}��J|w�fڹ��Vz��[�Ί���;Cg-���k�(�݈jC���Q�u�ѿ�b���R�Pw�[YOt�m���/��8��8�GrFQ�Q.ҳ� �A�g�mA���(M�k�6��^֩8�9��}�\�&|ˤd��c�,>�T���5��:v`e��*�����7�^��(y�ڶ"=y� ےu
����"���*׾��{!jn�8��b��c�c@���r����
9��ſflW �Iz�e0�fq�A�����j5 �D�S�� &�\���)}���D�,��<j/����X/����X�92��G�GP7�	\��#í p�M�t���T���4IUBW,��@�_]��#��[N�#|s5췇�e5�;�s�lp����>{wIW{�l�p����w= }�����0}�KK�_4�	�B$�F���S`"gS�?��3w8�V��O�	��R�Xn2|f�k�&��1jX|���hk���`x�U���mH;S&̽�>�?�۹��ئ��[�V��Q����(�S�d�MQ�{ui�d^h:���G@R�~s?lx-V��[�A[�i�?��;���\�@J��(K��ΏY
�Xv���}\,Q�,�\U�'�e��ٗGl�q���s�������x����	�Q�r��鈢���*a�ĩ�v���� ��6]�������O�uW�xW���I      7   �   x�E�Mn1F��)��T����]W���`�4��'��������o�Ǯ�z8B��Ue�V�fd`\%/�P�P#	UNO;�I}��@�Z��;�3B7�4�I�R�����(��oL��cL�B'��yT��ٰ�E�'����o�+G��ʶ�Z�c��g��^w⍬ц8��Fp,��y��'�3�����R��f`      =   �  x�uW�z�6<CO��6�$��h˞��ώ���LBb
��=�)����'�j�$(��4�(tWWu��WUk%��.�QXq�^Z�f���"�^̆����Nz���JԼ�����SF���Ե�fX�Mf�Z:Ui�r��M��6#�|��]��Z)����=!;.t��:L�����'^4�2`�٭�����Y����|�=�?!gs�X��so����5����Rx��R�!��f�_�����Qj/<2L�}3�b�>��Э���e��E��g��=ɽ(<�ښR�
'�$p9[��cc�;�)���xU��m��8��`.�,�eY�_�*$��GZ�Ѡ�c
z�P�Ğk�,So���)�ZF�W�J��g�<*�!U��8['Мm�W� �h|��[s���t�f_�w��U[�1����`ʙ�#{d+�Y���Cن��6|{���a5P��c
�O@�+ݚ�Dʜ�JL���͉
��ŏ�_~gDP�:c���Y�v�J�b�`������[]��!�,cפ�o��"�<��A�� ��E{��~ۥ~W��:�N�Ɇ�=	����+��@χc:+>g$�k�.k�B� -?x1©M�
D����Uu�o������d1O�̧s���wo!� �/���q��c[{���3�-��'=�� }į+~-���[�3��')v�U�6Pկ�	H�ԫ#����3r֕�:��ec�}\� �6�F;���k��-�{�Ϩ&��Ҡ��2�#�JP���F�3q�l8�-G%|m��lM�pAт�W�AR�O�i%��H����$�'Y�����6FCK�MG�z��R���&}S�w�� �rv����+[N1Ы���E.ȱEk��]���cb����� �u�h�m����O�X2��ES����6ݘ��s�{{�^�B�(��1�Y��yK$�u��
삑;�<��e�;��a>4���R�4�*l�N "f��؟�-�) �M�y����ӷ�)��,�&U��eSW�3��=���a����j�:P�5��d��������o���A���ʚ���Y#̓��g���i?�
� R0t�k��j“�Mא��81�Lwg���1�4����$*Q���-�g��m�%���)�7{�6q�v�K+��by��2Zap��4Hsa�Pǣq@.v1�����eے�Щ��oq;AB�mb2LD�B#��<\&e-��k�#�]�W�}������b� \��(�V��>ˤ�!�׮mc}���UYł�3�mI�k}���*廊�
V'v� ������g��|��<���P�A_۝t1�q@&_����3L�z�O�Kpp������FLGm�|��������쮘8�p��Y��*2�٩�d�u4֕p�� A[��'�@q�<5�sI�����J��j��﫫d����>�WA	�p��(��>��ߦkظ:=h�Qe������_����7���'�&Vl=j�׸I��.Q�%|����U������(���}�����Q�Ó�&<�P���Id�h�;��Yz���Q�_��u[Q�Uܣ�	��э�?7�~uC���0������b�=5#�7��M�f �D�f ]e����v��P>Y��W��QiY�`�3�<A����B�C�*W���s� H�#c���8����^��j����^�w
�us4��˞�(��Z��]t�*,����M��dBy�uN�9������|~��p0U���z�����nq��^�o�W�n���{�.9�u��K9 ˈ�]�=]�P��7��6�ŀ,dݽWF����eA��x�z��RM2��"'J�K��)�P�O�^�m������t�nFW�����a\��38j�S� �)2w�d �_�Qk�:$k?��B!sਇ]�R�(z�������L&�
w      ?   G   x�˱�0�Z&�dюw��s�]|A����(jZtT�
Ӣ�rhgL�t�jd�lt�m����G���.      ;     x�]P�N�@<;_�_�h���P*�"
*B\����(�F�M+�z��^z��X��c��[2��X��!)���c��&�%�l�P�u㓃"2�w�r����:'0wkOfÁ�Uq"��I�:�`�5��ѳ%����C�-Eu-H|�껆��Ͳ��S�ٝͽ�G4mڶ5��з4k։g1S��GRvObIf�Qw���#���;X"3:��0�����ܵ�]�O置�S�a^y;�5��ɣKX�>�ɼA�RA�>>�	a�N��Y��� �!      >   �   x�%��C1Ϧ�����%�ב��F�9GE���v�c����b��sb����#�8�F��aG��"۱Dd8���8���"����~)_��^ȍ�*v�jKd�e��-"�P]Y!��i<���6V4
#��� d��52��g���t��*�j�����y@��[��[wQ�W9���'"~:%8_     