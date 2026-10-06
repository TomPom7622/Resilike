using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerScript : MonoBehaviour
{
    //skrypty
    //game manager
    private GameManager gameManager;
    //skrypt z logika kamer
    public CameraLogic _cameraLogic;
    //skryp z logika ekwipunku
    public InventoryLogic inventory;
    //skrypt do celowania
    private AimScript aimScript;


    //inputy gracza
    private DefaultPlayerActions _defaultPlayerActions;

    //model gracza
    private Rigidbody _rb;

    private Animator animator;

    //medel broni
    public GameObject Gun;
    //model strzelania
    private RaycastScript Fire;
    //obiekt bedacy bronia
    Classes.Item currentWeapon;

    //gracz
    public Classes.Player player;
    
    public RaycastHit hit;

    //stale, ktore dotycza calej gry
    public static float _movementSpeed = 12f;
    public static float _turningSpeed = 180f;
    private float runSpeed=1f;

    private bool aiming;
    public bool isOnFloor;


    //co sie dzieje od razu
    private void Awake()
    {
        //stworzenie gracza
        player = new Classes.Player();
        //przypisanie zmiennych
        //kontrolki
        _defaultPlayerActions = new DefaultPlayerActions();
        //model
        _rb = GetComponent<Rigidbody>();
        //zachowanie kamer
        _cameraLogic = GameObject.FindGameObjectWithTag("GameController").GetComponent<CameraLogic>();
        //bron i jej wylaczenie
        Gun = GameObject.FindGameObjectWithTag("Gun");
        Gun.SetActive(false);
        inventory = GameObject.FindGameObjectWithTag("GameController").GetComponent<InventoryLogic>();
        aimScript = gameObject.GetComponent<AimScript>();
        Fire =Gun.GetComponentInChildren<RaycastScript>();
        animator = gameObject.GetComponent<Animator>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        aiming = false;
        isOnFloor = false;
    }

    //przy wlaczeniu obiektu
    private void Start()
    {
        //wlacza mozliwosc poruszania sie i strzelania
        gameManager.EnableMovement();
        
    }

    //przy wylaczeniu obiektu
    private void OnDisable()
    {
        //wylacza mozliwosc poruszanie sie i strzelania
        gameManager.DisableMovement();
    }
    
    //określa akcje strzelania
    public void OnFire(InputAction.CallbackContext context)
    {
        if(aiming) 
        {
            int currentAmmo = currentWeapon.Quant;
            if (currentAmmo > 0)
            {
                //zminiejsza quant amunicji
                inventory.AmmoDecreseOnFire(currentWeapon,1);
                //tworzy pocisk
                Fire.OnFire();
                currentAmmo--;
                currentWeapon.Quant = currentAmmo;
                Debug.Log("Fire");
            }
            else { Debug.Log("Empty"); }
            
        }
    }

    public void Aim(InputAction.CallbackContext context)
    {
        if (Gun.activeSelf)
        {
            animator.SetBool("IsAiming", true);
            aiming = true;
            aimScript.EnableAim();
        }
    }

    //funkcja odpowidzialna za wyposazanie broni
    public void Equip(Classes.Item weapon)
    {
        currentWeapon = weapon;

        if (currentWeapon != null)
        {
            //pokazuje model broni
            Gun.SetActive(true);
        }
        else
        {
            Gun.SetActive(false);
        }
    }
    public Vector3 GetCoordinates()
    {
        return gameObject.transform.position;
    }
    public void SetCoordinates(Vector3 value)
    {
        gameObject.transform.position = value;
    }
    public void InputOnOrOf()
    {
        if (_defaultPlayerActions.Player.enabled)
            _defaultPlayerActions.Player.Disable();
        else
            _defaultPlayerActions.Player.Enable();
    }

    public void PlayerDeath()
    {
        _defaultPlayerActions.Player.Disable();
    }

    //dziala w kazdej klatce
    private void FixedUpdate()
    {
        //celowanie
        if(!gameManager.aimAction.IsPressed())
        {
            animator.SetBool("IsAiming", false);
            aiming = false;
            aimScript.DisableAim();
        }
        else if(gameManager.aimAction.IsPressed() && Gun.activeSelf) 
            animator.SetBool("IsAiming", true);

        //bieganie
        if (gameManager.runAction.IsPressed())
            runSpeed = 2f; 
        else
            runSpeed = 1f;

        //poruszanie sie
        Vector2 tankMove = gameManager.tankAction.ReadValue<Vector2>();
        Vector3 tmp = tankMove.y * _movementSpeed * transform.forward * runSpeed;
        if (IsOnSlope()) 
        {
            Debug.Log("on slope");
            tmp = CalculateSlope(transform.forward) * tankMove.y * _movementSpeed;
        }
        Vector3 vel = _rb.linearVelocity;
        vel.x = tmp.x;
        vel.z = tmp.z;    
        vel.y = tmp.y;
        
        //rampy
        if (!isOnFloor)
        {
             Debug.Log("isnt on floor");
             vel.y = -10; 
        }
        _rb.linearVelocity = vel;

        //obrot
        if(tankMove.x != 0)
        {
            Vector3 angleVelocity = new Vector3(0, tankMove.x * _turningSpeed, 0);
            Quaternion deltaRotation = Quaternion.Euler(angleVelocity * Time.fixedDeltaTime);
            _rb.MoveRotation(_rb.rotation * deltaRotation);
        }
        else
        {
            _rb.angularVelocity= new Vector3(0,0,0);
        }
    }

    //zmiana kamery
    private void OnTriggerEnter(Collider other)
    {   
        if (other.tag.Substring(0, 4) == "CamT")
        {
            //Debug.Log(other.tag);
            //Debug.Log(other.name);
            _cameraLogic.cameraChange(other.name.Substring(4));
        }
    }

    private bool IsOnSlope()
    {
        
        if(Physics.Raycast(transform.position,-transform.up,out hit, Mathf.Infinity,1<<3))
        {
            if(hit.collider.CompareTag("Slope"))
            {
                float angle = Vector3.Angle(Vector3.up, hit.normal);
                return angle > 0;
            }
        }
        return false;
    }
    
    private Vector3 CalculateSlope(Vector3 move)
    {
        return Vector3.ProjectOnPlane(move, hit.normal).normalized;
    }
    
}
